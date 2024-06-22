using CandidateHub.Api.Commons.Models;

namespace CandidateHub.Api.V1.Candidates.Models;

public class CandidateFilterModel : PagedFilterModel
{
    public Guid? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    
}