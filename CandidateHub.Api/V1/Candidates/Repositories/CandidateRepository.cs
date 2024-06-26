using System.Data;
using System.Text;
using CandidateHub.Api.Commons.Exceptions;
using CandidateHub.Api.Data.MSSQL.Connections.Interfaces;
using CandidateHub.Api.V1.Candidates.Entities;
using CandidateHub.Api.V1.Candidates.Models;
using CandidateHub.Api.V1.Candidates.Repositories.Interfaces;
using Dapper;
using ServiceLocator;

namespace CandidateHub.Api.V1.Candidates.Repositories;

[Service(ServiceLifetime.Scoped)]
public class CandidateRepository(IDatabaseConnection _databaseConnection, ILogger<CandidateRepository> _logger) : ICandidateRepository
{
    public async Task<bool> IsEmailExist(string email)
    { 
        var sql = @"SELECT COUNT(*) FROM Candidates WHERE Email = @email";
        var connection = await _databaseConnection.GetConnection();
        var count = connection.ExecuteScalar<int>(sql, param: new { email });
        return count == 1;
    }
    
    public async Task<Candidate> Create(CandidateCreateOrUpdateModel model)
    {
        if (await IsEmailExist(model.Email))
        {
            throw new BusinessException("Candidate already registered with this email", 400);
        }
        
        var sql = @"
INSERT INTO Candidates (
    Id, FirstName, LastName, PhoneNumber, Email, CallTimeInterval, LinkedinProfile, GithubProfile, Comment, CreatedAt
) VALUES ( @Id, @FirstName, @LastName, @PhoneNumber, @Email, @CallTimeInterval, @LinkedinProfile, @GithubProfile, @Comment, GETUTCDATE() );
";

        var parameters = new
        {
            Id = Guid.NewGuid(),
            model.FirstName, model.LastName, model.Email, model.CallTimeInterval, model.LinkedinProfile, PhoneNumber = model.PhoneNumber,
            model.GithubProfile, model.Comment
        };

        IDbTransaction transaction = null;
        try
        {
            var connection = await _databaseConnection.GetConnection();
            transaction = connection.BeginTransaction();

            await connection.ExecuteAsync(sql, transaction: transaction, param: parameters);
            transaction.Commit();
            return (await GetById(parameters.Id))!;
        }
        catch (Exception e)
        {
            _logger.LogError(e,$"Error while creating candidate: {e.Message}");    
            transaction?.Rollback();
            throw new BusinessException("Internal error occured while creating Candidate, please try later", 500);
        }
    }

    public async Task<Candidate> Update(CandidateCreateOrUpdateModel model)
    {
        if (!await IsEmailExist(model.Email))
        {
            throw new BusinessException("Cannot update not registered candidate", 400);
        }
        
        var sql = @"
UPDATE Candidates
SET
    FirstName = @FirstName,
    LastName = @LastName,
    PhoneNumber = @PhoneNumber,
    CallTimeInterval = @CallTimeInterval,
    LinkedinProfile = @LinkedinProfile,
    GithubProfile = @GithubProfile,
    Comment = @Comment,
    UpdatedAt = GETUTCDATE()
WHERE
    Email = @Email;
";

        var connection = await _databaseConnection.GetConnection();
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                await connection.ExecuteAsync(sql, transaction: transaction, param: model);
                transaction.Commit();
                return (await GetByEmail(model.Email))!;
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"Error while creating candidate: {e.Message}");    
                transaction?.Rollback();
                throw new BusinessException("Internal error occured while updating Candidate, please try later", 500);
            }
        }
    }

    private async Task<Candidate?> GetById(Guid id)
    {
        var sql = @"
SELECT Id, FirstName, LastName, PhoneNumber, Email, CallTimeInterval, LinkedinProfile, GithubProfile, Comment, CreatedAt, UpdatedAt
FROM Candidates WHERE Id = @id";
        var connection = await _databaseConnection.GetConnection();
        var candidates = await connection.QueryAsync<Candidate>(sql, param: new { id });
        return candidates.FirstOrDefault();
    }
    private async Task<Candidate?> GetByEmail(string email)
    {
        var sql = @"
SELECT Id, FirstName, LastName, PhoneNumber, Email, CallTimeInterval, LinkedinProfile, GithubProfile, Comment, CreatedAt, UpdatedAt
FROM Candidates WHERE Email = @email";
        var connection = await _databaseConnection.GetConnection();
        var candidates = await connection.QueryAsync<Candidate>(sql, param: new { email });
        return candidates.FirstOrDefault();
    }
    
    public async Task<List<Candidate>> GetByFilter(CandidateFilterModel model)
    {
        model.CheckOrSetDefaults();
        var sql = @"
SELECT [Id]
      ,[FirstName]
      ,[LastName]
      ,[PhoneNumber]
      ,[Email]
      ,[CallTimeInterval]
      ,[LinkedinProfile]
      ,[GithubProfile]
      ,[Comment]
      ,[CreatedAt]
      ,[UpdatedAt]
  FROM [CandidateHubDatabase].[dbo].[Candidates]
WHERE @where

ORDER BY Id
OFFSET (@Page - 1) * @Size ROWS
FETCH NEXT @Size ROWS ONLY
 
";

        sql = sql.Replace("@where", GetQuery(model));

        //throw new Exception(sql);
        var connection = await _databaseConnection.GetConnection();

        var result = await connection.QueryAsync<Candidate>(sql, model);
        return result.ToList();
    }

    public async Task<int> GetCount(CandidateFilterModel model)
    {
        var sql = @"
SELECT COUNT(Id)
  FROM [CandidateHubDatabase].[dbo].[Candidates]
WHERE @where 
";

        sql = sql.Replace("@where", GetQuery(model));
        
        var connection = await _databaseConnection.GetConnection();
        return await connection.ExecuteScalarAsync<int>(sql, model);
    }

    public string GetQuery(CandidateFilterModel model)
    {
        var filter = new StringBuilder();
        filter.Append("1=1");
        if (model.Id.HasValue && model.Id.Value != Guid.Empty)
        {
            filter.Append($" AND Id = @Id");
        }

        if (!string.IsNullOrEmpty(model.FirstName))
        {
            filter.Append($" AND LOWER(FirstName) LIKE {GetContainsPattern("@FirstName")}");
        }
        if (!string.IsNullOrEmpty(model.LastName))
        {
            filter.Append($" AND LOWER(LastName) LIKE {GetContainsPattern("@LastName")}");
        }
        
        if (!string.IsNullOrEmpty(model.Email))
        {
            filter.Append($" AND LOWER(Email) LIKE {GetContainsPattern("@Email")}");
        }
        if (!string.IsNullOrEmpty(model.PhoneNumber))
        {
            filter.Append($" AND LOWER(PhoneNumber) LIKE {GetContainsPattern("@PhoneNumber")}");
        }

        return filter.ToString();
    }

    private string GetContainsPattern(string variableName) => $"('%' + LOWER({variableName}) + '%')";
}
