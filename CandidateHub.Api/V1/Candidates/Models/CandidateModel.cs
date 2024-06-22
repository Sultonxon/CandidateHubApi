using CandidateHub.Api.V1.Candidates.Entities;

namespace CandidateHub.Api.V1.Candidates.Models;

public class CandidateModel
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
    
    public static  implicit operator CandidateModel(Candidate entity)
    {
        return new CandidateModel
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            PhoneNumber = entity.PhoneNumber,
            Email = entity.Email,
            CallTimeInterval = entity.CallTimeInterval,
            LinkedinProfile = entity.LinkedinProfile,
            GithubProfile = entity.GithubProfile,
            Comment = entity.Comment,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}