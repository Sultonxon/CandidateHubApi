using CandidateHub.Api.V1.Candidates.Entities;
using CandidateHub.Api.V1.Candidates.Models;
using CandidateHub.Api.V1.Candidates.Repositories.Interfaces;
using CandidateHub.Api.V1.Candidates.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace CandidateHub.Api.Tests.V1.Candidates.Services;

public class CandidateServiceTests
{
    #region CreateOrUpdate method tests
    
    [Fact]
    public async Task CreateOrUpdate_WhenEmailDoesNotExist_ShouldCreateNewCandidate()
    {
        var model = new CandidateCreateOrUpdateModel
        {
            FirstName = "firstName", LastName = "Last Name", PhoneNumber = "+1223455666", Email = $"testemail{Helpers.GetRandomEmailPrefix()}@gmail.com",
            CallTimeInterval = "10:00-19:00", GithubProfile = "https://github.com/name",
            LinkedinProfile = "https://linkedin.com/name"
        };
        var testStore = new List<Candidate>();
        var mockCandidateRepository = new Mock<ICandidateRepository>();

        mockCandidateRepository.Setup(x => x.Create(It.IsAny<CandidateCreateOrUpdateModel>()))
            .ReturnsAsync( (CandidateCreateOrUpdateModel model)=>
            {
                var candidate = new Candidate
                {
                    Id = Guid.NewGuid(), FirstName = model.FirstName, LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber, Email = model.Email, CallTimeInterval = model.CallTimeInterval,
                    GithubProfile = model.GithubProfile, LinkedinProfile = model.LinkedinProfile,
                    CreatedAt = DateTime.UtcNow
                };
                testStore.Add(candidate);
                return candidate;
            });
        
        var mockLogger = new Mock<ILogger<CandidateService>>();
        var candidateService = new CandidateService(mockCandidateRepository.Object, mockLogger.Object);

        var createdCandidate = await candidateService.CreateOrUpdate(model);
        Assert.NotNull(createdCandidate);
        Assert.True(testStore.Any(x => x.Email == createdCandidate.Email));
        Assert.Null(testStore.First(x => x.Email == createdCandidate.Email).UpdatedAt);
    }
    
    [Fact]
    public async Task CreateOrUpdate_WhenEmailExist_ShouldUpdateExistingCandidate()
    {
        var model = new CandidateCreateOrUpdateModel
        {
            FirstName = "firstName", LastName = "Last Name", PhoneNumber = "+1223455666", Email = $"testemail{Helpers.GetRandomEmailPrefix()}@gmail.com",
            CallTimeInterval = "10:00-19:00", GithubProfile = "https://github.com/name",
            LinkedinProfile = "https://linkedin.com/name"
        };
        
        
        var testStore = new List<Candidate>()
        {
            //candidate is already exist with this email
            new Candidate
            {
                Id = Guid.NewGuid(), FirstName = model.FirstName, LastName = model.LastName,
                PhoneNumber = model.PhoneNumber, Email = model.Email, CallTimeInterval = model.CallTimeInterval,
                GithubProfile = model.GithubProfile, LinkedinProfile = model.LinkedinProfile,
                CreatedAt = DateTime.UtcNow
            }
        };
        var mockCandidateRepository = new Mock<ICandidateRepository>();

        mockCandidateRepository.Setup(x => x.Update(It.IsAny<CandidateCreateOrUpdateModel>()))
            .ReturnsAsync( (CandidateCreateOrUpdateModel m)=>
            {
                var c = testStore.First(x => x.Email == m.Email);
                //updated at was null when it is created
                c.UpdatedAt = DateTime.UtcNow;
                
                return c;
            });
        
        mockCandidateRepository.Setup(x => x.IsEmailExist(It.IsAny<string>()))
            .ReturnsAsync( (string email)=>
            {
                 return testStore.Any(x => x.Email == email);
            });
        
        var mockLogger = new Mock<ILogger<CandidateService>>();
        var candidateService = new CandidateService(mockCandidateRepository.Object, mockLogger.Object);

        Assert.NotNull(candidateService);
        var updatedCandidate = await candidateService.CreateOrUpdate(model);
        Assert.NotNull(updatedCandidate);
        Assert.True(testStore.Any(x => x.Email == updatedCandidate.Email));
        Assert.NotNull(testStore.First(x => x.Email == updatedCandidate.Email).UpdatedAt);
    }
    #endregion
    
}