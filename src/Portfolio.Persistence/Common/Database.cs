using System.Data;
using Dapper;
using Microsoft.Extensions.Options;
using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Settings;
using Portfolio.Domain.Enums;

namespace Portfolio.Persistence.Common;

/// <summary>
/// Central data-access helper. Callers pass parameters as a plain hashtable
/// (name -> value, NO prefix); this class adds the <c>@P_</c> prefix to every
/// parameter and the standard <c>@P_StatusCode</c> / <c>@P_StatusMessage</c>
/// outputs, then executes the stored procedure.
///
/// Convention: stored-procedure parameters are named <c>@P_*</c> and any
/// DECLAREd local variables inside a procedure are named <c>@L_*</c>.
/// </summary>
public sealed class Database
{
    public const string Prefix = "P_";

    private readonly IDbConnectionFactory _connectionFactory;
    private readonly int _commandTimeout;

    public Database(IDbConnectionFactory connectionFactory, IOptions<DatabaseSettings> settings)
    {
        _connectionFactory = connectionFactory;
        _commandTimeout = settings.Value.CommandTimeoutSeconds;
    }

    /// <summary>Builds prefixed parameters + the status output parameters.</summary>
    private static DynamicParameters Build(
        IReadOnlyDictionary<string, object?> inputs,
        string? outIdKey = null,
        bool paged = false)
    {
        var p = new DynamicParameters();
        foreach (var kv in inputs)
        {
            p.Add(Prefix + kv.Key, kv.Value);
        }

        p.Add(Prefix + "StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        p.Add(Prefix + "StatusMessage", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

        if (outIdKey is not null)
        {
            p.Add(Prefix + outIdKey, dbType: DbType.Int32, direction: ParameterDirection.Output);
        }

        if (paged)
        {
            p.Add(Prefix + "TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add(Prefix + "TotalPages", dbType: DbType.Int32, direction: ParameterDirection.Output);
        }

        return p;
    }

    private static SpExecutionResult ReadStatus(DynamicParameters p, string? outIdKey = null) => new()
    {
        StatusCode = (SpStatusCode)p.Get<int>(Prefix + "StatusCode"),
        StatusMessage = p.Get<string>(Prefix + "StatusMessage") ?? string.Empty,
        OutId = outIdKey is null ? null : p.Get<int?>(Prefix + outIdKey)
    };

    private CommandDefinition Command(string proc, DynamicParameters p, CancellationToken ct) =>
        new(proc, p, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout,
            cancellationToken: ct);

    /// <summary>INSERT/UPDATE/DELETE style — returns status (+ optional new id).</summary>
    public async Task<SpExecutionResult> ExecuteAsync(
        string procedureName,
        IReadOnlyDictionary<string, object?> inputs,
        string? outIdKey = null,
        CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
        var p = Build(inputs, outIdKey);
        await connection.ExecuteAsync(Command(procedureName, p, cancellationToken)).ConfigureAwait(false);
        return ReadStatus(p, outIdKey);
    }

    /// <summary>Non-paged result set.</summary>
    public async Task<(SpExecutionResult Status, IReadOnlyList<T> Items)> QueryAsync<T>(
        string procedureName,
        IReadOnlyDictionary<string, object?> inputs,
        CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
        var p = Build(inputs);
        var items = (await connection.QueryAsync<T>(Command(procedureName, p, cancellationToken)).ConfigureAwait(false)).ToList();
        return (ReadStatus(p), items);
    }

    /// <summary>Single-row result set (e.g. GetById on a flat table).</summary>
    public async Task<(SpExecutionResult Status, T? Item)> QuerySingleAsync<T>(
        string procedureName,
        IReadOnlyDictionary<string, object?> inputs,
        CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
        var p = Build(inputs);
        var item = await connection.QueryFirstOrDefaultAsync<T>(Command(procedureName, p, cancellationToken)).ConfigureAwait(false);
        return (ReadStatus(p), item);
    }

    /// <summary>Paged result set — also reads @P_TotalRecords / @P_TotalPages.</summary>
    public async Task<(SpExecutionResult Status, IReadOnlyList<T> Items, int TotalRecords, int TotalPages)> QueryPagedAsync<T>(
        string procedureName,
        IReadOnlyDictionary<string, object?> inputs,
        CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
        var p = Build(inputs, paged: true);
        var items = (await connection.QueryAsync<T>(Command(procedureName, p, cancellationToken)).ConfigureAwait(false)).ToList();
        return (
            ReadStatus(p),
            items,
            p.Get<int>(Prefix + "TotalRecords"),
            p.Get<int>(Prefix + "TotalPages"));
    }

    /// <summary>Multiple result sets — the mapper reads the grid; status is read after.</summary>
    public async Task<(SpExecutionResult Status, TResult Result)> QueryMultipleAsync<TResult>(
        string procedureName,
        IReadOnlyDictionary<string, object?> inputs,
        Func<SqlMapper.GridReader, Task<TResult>> map,
        CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
        var p = Build(inputs);
        TResult result;
        using (var grid = await connection.QueryMultipleAsync(Command(procedureName, p, cancellationToken)).ConfigureAwait(false))
        {
            result = await map(grid).ConfigureAwait(false);
        }
        return (ReadStatus(p), result);
    }
}
