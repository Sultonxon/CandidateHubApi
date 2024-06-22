using CandidateHub.Api.V1.Candidates.Models;

namespace CandidateHub.Api.V1.Candidates.Services.Interfaces;

public interface ICandidateService
{
    Task<CandidateModel> CreateOrUpdate(CandidateCreateOrUpdateModel model);
}