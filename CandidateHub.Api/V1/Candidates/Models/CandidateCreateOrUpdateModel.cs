using System.ComponentModel.DataAnnotations;

namespace CandidateHub.Api.V1.Candidates.Models;

public class CandidateCreateOrUpdateModel
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    [MaxLength(50)]
    public string PhoneNumber { get; set; }
    [Required]
    [MaxLength(100)]
    public string Email { get; set; }
    [MaxLength(50)]
    public string CallTimeInterval { get; set; }
    [MaxLength(255)]
    public string LinkedinProfile { get; set; }
    [MaxLength(255)]
    public string GithubProfile { get; set; }
    [Required]
    [MaxLength(1000)]
    public string Comment { get; set; }
}