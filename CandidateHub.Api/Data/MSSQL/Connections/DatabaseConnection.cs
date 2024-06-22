using System.Data;
using CandidateHub.Api.Data.MSSQL.Connections.Interfaces;
using CandidateHub.Api.Data.MSSQL.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using ServiceLocator;

namespace CandidateHub.Api.Data.MSSQL.Connections;

[Service(ServiceLifetime.Scoped)]
public class DatabaseConnection : IDatabaseConnection
{
    private readonly SqlServerOptions _options;
    private SqlConnection? _connection;

    public DatabaseConnection(IOptions<SqlServerOptions> options)
    {
        _options = options.Value;
    }
    
    public async Task<IDbConnection> GetConnection() {
        if (_connection is null || _connection.State == ConnectionState.Broken ||
            _connection.State == ConnectionState.Closed)
        {
            _connection = new SqlConnection(_options.ConnectionString);
            await _connection.OpenAsync();
        }

        return _connection;
    }
}