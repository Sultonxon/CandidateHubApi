using System.Text;
using System.Text.Json;
using CandidateHub.Api.Commons.Exceptions;
using ServiceLocator;

namespace CandidateHub.Api.Middlewares;

[Service(ServiceLifetime.Scoped)]
public class ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> _logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (BusinessException e)
        {
            _logger.LogError(e, e.Message);
            context.Response.StatusCode = e.Code;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
            {
                Message = e.Message
            })));
        }
        
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            context.Response.StatusCode = 500;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
            {
                Message = "Internal Server Error"
            })));
        }
    }
}