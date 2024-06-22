using CandidateHub.Api.Commons.Exceptions;
using CandidateHub.Api.Data.MSSQL.Connections.Interfaces;
using CandidateHub.Api.V1.Candidates.Models;
using CandidateHub.Api.V1.Candidates.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Moq;

namespace CandidateHub.Api.Tests.V1.Candidates.Repositories;

public class CandidateRepositoryTests
{
    #region IsEmailExist

    [Fact]
    public async Task IsEmailExist_Tests()
    {
        var model = new CandidateCreateOrUpdateModel
        {
            FirstName = "firstName", LastName = "Last Name", PhoneNumber = "+1223455666", Email = $"testemail{Helpers.GetRandomEmailPrefix()}@gmail.com",
            CallTimeInterval = "10:00-19:00", GithubProfile = "https://github.com/name",
            LinkedinProfile = "https://linkedin.com/name"
        };
        
        var mockConnection = new Mock<IDatabaseConnection>();
        var mockLogger = new Mock<ILogger<CandidateRepository>>();

        mockConnection.Setup(c => c.GetConnection()).ReturnsAsync(() =>
        {
            var c = new SqlConnection(GlobalTestConstants.ConnectionString);
            c.Open();
            return c;
        });

        var repository = new CandidateRepository(mockConnection.Object, mockLogger.Object);

        Assert.False(await repository.IsEmailExist(model.Email));
        await repository.Create(model);
        Assert.True(await repository.IsEmailExist(model.Email));
    }

    #endregion
    
    
    #region Update method tests

    [Fact]
    public async Task Update_InvalidEmail_UpdatesCandidate()
    {
        var model = new CandidateCreateOrUpdateModel
        {
            FirstName = "firstName", LastName = "Last Name", PhoneNumber = "+1223455666", Email = $"testemail{Helpers.GetRandomEmailPrefix()}@gmail.com",
            CallTimeInterval = "10:00-19:00", GithubProfile = "https://github.com/name",
            LinkedinProfile = "https://linkedin.com/name"
        };
        
        var mockConnection = new Mock<IDatabaseConnection>();
        var mockLogger = new Mock<ILogger<CandidateRepository>>();

        mockConnection.Setup(c => c.GetConnection()).ReturnsAsync(() =>
        {
            var c = new SqlConnection(GlobalTestConstants.ConnectionString);
            c.Open();
            return c;
        });

        var repository = new CandidateRepository(mockConnection.Object, mockLogger.Object);
        // Assert
        var e = await Assert.ThrowsAsync<BusinessException>(async () => await repository.Update(model));
    }
    
    [Fact]
    public async Task Update_ValidEmail_UpdatesCandidate()
    {
        // Arrange
        var model = new CandidateCreateOrUpdateModel
        {
            FirstName = "firstName", LastName = "Last Name", PhoneNumber = "+1223455666", Email = $"testemail@gmail.com",
            CallTimeInterval = "10:00-19:00", GithubProfile = "https://github.com/name",
            LinkedinProfile = "https://linkedin.com/name"
        };
        
        var mockConnection = new Mock<IDatabaseConnection>();
        var mockLogger = new Mock<ILogger<CandidateRepository>>();

        mockConnection.Setup(c => c.GetConnection()).ReturnsAsync(() =>
        {
            var c = new SqlConnection(GlobalTestConstants.ConnectionString);
            c.Open();
            return c;
        });

        var repository = new CandidateRepository(mockConnection.Object, mockLogger.Object);

        // Act
        if (!await repository.IsEmailExist(model.Email))
        {
            await repository.Create(model);
        }
        var updatedCandidate = await repository.Update(model);

        // Assert
        Assert.NotNull(updatedCandidate);
        Assert.True(updatedCandidate.Email == model.Email);

    }
    
    #endregion


    #region Create method tests
    
    [Fact]
    public async Task Create_ValidCandidate_CreatesCandidate()
    {
        // Arrange
        var model = new CandidateCreateOrUpdateModel
        {
            FirstName = "firstName", LastName = "Last Name", PhoneNumber = "+1223455666", Email = $"testemail{Helpers.GetRandomEmailPrefix()}@gmail.com",
            CallTimeInterval = "10:00-19:00", GithubProfile = "https://github.com/name",
            LinkedinProfile = "https://linkedin.com/name"
        };

        var mockConnection = new Mock<IDatabaseConnection>();
        var mockLogger = new Mock<ILogger<CandidateRepository>>();

        mockConnection.Setup(c => c.GetConnection()).ReturnsAsync(() =>
        {
            var c = new SqlConnection(GlobalTestConstants.ConnectionString);
            c.Open();
            return c;
        });

        var service = new CandidateRepository(mockConnection.Object, mockLogger.Object);

        // Act
        var createdCandidate = await service.Create(model);

        // Assert
        Assert.NotNull(createdCandidate);
        Assert.True(createdCandidate.Email == model.Email);
    }

    [Fact]
    public async Task Create_DuplicateEmail_ThrowsBusinessException()
    {
        // Arrange
        var model = new CandidateCreateOrUpdateModel
        {
            FirstName = "firstName", LastName = "Last Name", PhoneNumber = "+1223455666", Email = $"testemail{Helpers.GetRandomEmailPrefix()}@gmail.com",
            CallTimeInterval = "10:00-19:00", GithubProfile = "https://github.com/name",
            LinkedinProfile = "https://linkedin.com/name"
        };

        var mockConnection = new Mock<IDatabaseConnection>();
        var mockLogger = new Mock<ILogger<CandidateRepository>>();

        mockConnection.Setup(c => c.GetConnection()).ReturnsAsync(() =>
        {
            var c = new SqlConnection(GlobalTestConstants.ConnectionString);
            c.Open();
            return c;
        });
        
        var repository = new CandidateRepository(mockConnection.Object, mockLogger.Object);

        //create candidate
        await repository.Create(model);
        // Act & Assert, indicate that candidate will not be created
        await Assert.ThrowsAsync<BusinessException>(async () => await repository.Create(model));
    }
    #endregion
}