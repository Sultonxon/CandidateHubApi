using CandidateHub.Api.Commons.Exceptions;
using CandidateHub.Api.V1.Candidates.Models;
using CandidateHub.Api.V1.Candidates.Repositories.Interfaces;
using CandidateHub.Api.V1.Candidates.Services.Interfaces;
using ServiceLocator;

namespace CandidateHub.Api.V1.Candidates.Services;

[Service(ServiceLifetime.Scoped)]
public class CandidateService(ICandidateRepository _candidateRepository, ILogger<CandidateService> _logger) : ICandidateService
{
    public async Task<CandidateModel> CreateOrUpdate(CandidateCreateOrUpdateModel model)
    {
        _logger.LogInformation("Creating or updating candidate data");

        var candidateEntity = await _candidateRepository.IsEmailExist(model.Email)
            ? await _candidateRepository.Update(model)
            : await _candidateRepository.Create(model);
        return candidateEntity;
    }
}
