using System.Data;
using Dapper;
using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Domain.Enums;

namespace Portfolio.Persistence.Common;

public abstract class RepositoryBase
{
    protected readonly IDbConnectionFactory ConnectionFactory;
    protected readonly StoredProcedureExecutor Executor;
    protected readonly int CommandTimeout;

    protected RepositoryBase(
        IDbConnectionFactory connectionFactory,
        StoredProcedureExecutor executor,
        Microsoft.Extensions.Options.IOptions<Portfolio.Application.Common.Settings.DatabaseSettings> settings)
    {
        ConnectionFactory = connectionFactory;
        Executor = executor;
        CommandTimeout = settings.Value.CommandTimeoutSeconds;
    }

    protected static void AddStatusOutputs(DynamicParameters parameters)
    {
        parameters.Add("StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("StatusMessage", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);
    }

    protected static SpExecutionResult ReadStatus(DynamicParameters parameters, string? outParam = null, int? outId = null)
    {
        return new SpExecutionResult
        {
            StatusCode = (SpStatusCode)parameters.Get<int>("StatusCode"),
            StatusMessage = parameters.Get<string>("StatusMessage") ?? string.Empty,
            OutId = outParam is null ? outId : parameters.Get<int?>(outParam)
        };
    }

    protected async Task<(SpExecutionResult Status, IReadOnlyList<T> Items)> QueryListAsync<T>(
        string procedureName,
        DynamicParameters parameters,
        CancellationToken cancellationToken)
    {
        using var connection = await ConnectionFactory.CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
        AddStatusOutputs(parameters);

        var items = (await connection.QueryAsync<T>(
            new CommandDefinition(procedureName, parameters, commandType: CommandType.StoredProcedure,
                commandTimeout: CommandTimeout, cancellationToken: cancellationToken)).ConfigureAwait(false)).ToList();

        return (ReadStatus(parameters), items);
    }

    protected async Task<SpExecutionResult> ExecuteCommandAsync(
        string procedureName,
        DynamicParameters parameters,
        string? outIdParameterName,
        CancellationToken cancellationToken)
    {
        using var connection = await ConnectionFactory.CreateConnectionAsync(cancellationToken).ConfigureAwait(false);

        if (outIdParameterName is not null)
        {
            return await Executor.ExecuteWithOutputAsync(connection, procedureName, parameters,
                outIdParameterName, cancellationToken).ConfigureAwait(false);
        }

        await connection.ExecuteAsync(
            new CommandDefinition(procedureName, parameters, commandType: CommandType.StoredProcedure,
                commandTimeout: CommandTimeout, cancellationToken: cancellationToken)).ConfigureAwait(false);

        return ReadStatus(parameters);
    }

    protected async Task<(SpExecutionResult Status, IReadOnlyList<T> Items, int TotalRecords, int TotalPages)> QueryPagedAsync<T>(
        string procedureName,
        DynamicParameters parameters,
        CancellationToken cancellationToken)
    {
        using var connection = await ConnectionFactory.CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
        AddStatusOutputs(parameters);
        parameters.Add("TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("TotalPages", dbType: DbType.Int32, direction: ParameterDirection.Output);

        var items = (await connection.QueryAsync<T>(
            new CommandDefinition(procedureName, parameters, commandType: CommandType.StoredProcedure,
                commandTimeout: CommandTimeout, cancellationToken: cancellationToken)).ConfigureAwait(false)).ToList();

        return (
            ReadStatus(parameters),
            items,
            parameters.Get<int>("TotalRecords"),
            parameters.Get<int>("TotalPages"));
    }
}
