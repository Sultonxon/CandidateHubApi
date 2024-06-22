namespace CandidateHub.Api.Tests.V1.Candidates;

public static class Helpers
{
    public static string GetRandomEmailPrefix() => Guid.NewGuid().ToString().Replace("-", "").ToLower();
}