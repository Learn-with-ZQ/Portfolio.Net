using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Settings;

namespace Portfolio.Persistence.Connection;

public sealed class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly DatabaseSettings _settings;

    public SqlConnectionFactory(IOptions<DatabaseSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new SqlConnection(_settings.ConnectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        return connection;
    }
}
