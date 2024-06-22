using CandidateHub.Api.V1.Candidates.Entities;
using CandidateHub.Api.V1.Candidates.Models;

namespace CandidateHub.Api.V1.Candidates.Repositories.Interfaces;

public interface ICandidateRepository
{
    Task<bool> IsEmailExist(string email);
    Task<Candidate> Create(CandidateCreateOrUpdateModel model);
    Task<Candidate> Update(CandidateCreateOrUpdateModel model);
}
