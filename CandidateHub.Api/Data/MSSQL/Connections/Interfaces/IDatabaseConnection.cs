using System.Data;

namespace CandidateHub.Api.Data.MSSQL.Connections.Interfaces;

public interface IDatabaseConnection
{
    Task<IDbConnection> GetConnection();
}