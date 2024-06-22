namespace CandidateHub.Api.V1.Candidates.Entities;

public class Candidate
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string CallTimeInterval { get; set; }
    public string LinkedinProfile { get; set; }
    public string GithubProfile { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
