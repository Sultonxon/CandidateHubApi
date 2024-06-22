namespace CandidateHub.Api.Commons.Models;

public class PagedFilterModel
{
    public int Page { get; set; }
    public int Size { get; set; }

    public void CheckOrSetDefaults()
    {
        if (Page <= 0) Page = 1;
        if (Size <= 0) Size = 30;
    }
}