using CandidateHub.Api.Commons.Models;
using CandidateHub.Api.V1.Candidates.Models;

namespace CandidateHub.Api.V1.Candidates.Services.Interfaces;

public interface ICandidateService
{
    Task<CandidateModel> CreateOrUpdate(CandidateCreateOrUpdateModel model);
    Task<PagedResult<CandidateModel>> GetByFilter(CandidateFilterModel model);
}