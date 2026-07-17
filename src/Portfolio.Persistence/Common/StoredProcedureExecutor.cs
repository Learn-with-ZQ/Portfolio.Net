using System.Data;
using Dapper;
using Microsoft.Extensions.Options;
using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Settings;
using Portfolio.Domain.Enums;

namespace Portfolio.Persistence.Common;

public sealed class StoredProcedureExecutor
{
    private readonly int _commandTimeout;

    public StoredProcedureExecutor(IOptions<DatabaseSettings> settings)
    {
        _commandTimeout = settings.Value.CommandTimeoutSeconds;
    }

    public async Task<SpExecutionResult> ExecuteWithOutputAsync(
        IDbConnection connection,
        string procedureName,
        DynamicParameters parameters,
        string outIdParameterName,
        CancellationToken cancellationToken = default)
    {
        await connection.ExecuteAsync(
            new CommandDefinition(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout,
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        return new SpExecutionResult
        {
            StatusCode = (SpStatusCode)parameters.Get<int>("StatusCode"),
            StatusMessage = parameters.Get<string>("StatusMessage") ?? string.Empty,
            OutId = parameters.Get<int?>(outIdParameterName)
        };
    }

    public DynamicParameters CreateOutputParameters()
    {
        var parameters = new DynamicParameters();
        parameters.Add("StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("StatusMessage", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);
        return parameters;
    }
}
