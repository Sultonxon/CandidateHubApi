using CandidateHub.Api.Commons.Exceptions;
using CandidateHub.Api.Commons.Models;
using CandidateHub.Api.V1.Candidates.Models;
using CandidateHub.Api.V1.Candidates.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CandidateHub.Api.V1.Candidates.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class CandidateController(ICandidateService candidateService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CandidateCreateOrUpdateModel>(200)]
    public async Task<IActionResult> CreateOrUpdate([FromBody] CandidateCreateOrUpdateModel model)
    {
        var candidate = await candidateService.CreateOrUpdate(model);
        return Ok(candidate);
    }

    [HttpGet]
    [ProducesResponseType<PagedResult<CandidateModel>>(200)]
    public async Task<IActionResult> GetByFilter([FromQuery]CandidateFilterModel model)
    {
        var result = await candidateService.GetByFilter(model);
        return Ok(result);
    }
}
