using System.Net;
using UserAuth.Exceptions;

namespace UserAuth.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var code = HttpStatusCode.InternalServerError;
        var resultObj = new { error = "An unexpected error occurred." };

        switch (ex)
        {
            case UserNotFoundException _:
                code = HttpStatusCode.NotFound;
                resultObj = new { error = ex.Message };
                break;
            case InvalidPasswordException _:
                code = HttpStatusCode.BadRequest;
                resultObj = new { error = ex.Message };
                break;
            case EmailAlreadyInUseException _:
            case UserAlreadyExistsException _:
                code = HttpStatusCode.BadRequest;
                resultObj = new { error = ex.Message };
                break;
            case OperationNotAllowedException _:
                code = HttpStatusCode.Forbidden;
                resultObj = new { error = ex.Message };
                break;
            default:
                resultObj = new { error = ex.Message };
                break;
        }
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsJsonAsync(resultObj);
    }
}