namespace CandidateHub.Api.Commons.Exceptions;

public class BusinessException : Exception
{
    public int Code { get; set; }

    public BusinessException(string message, int code) : base(message)
    {
        Code = code;        
    }
}