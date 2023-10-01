using Microsoft.AspNetCore.Mvc.Filters;

namespace OrderingApi.AppFilters;



public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    private readonly ILogger<HttpResponseExceptionFilter> logger;

    public HttpResponseExceptionFilter(ILogger<HttpResponseExceptionFilter> logger)
    {
        this.logger = logger;
    }

    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is Exception exception)
        {
            logger.LogError($"[{context?.HttpContext?.Request?.Method}] {context?.HttpContext?.Request?.Path}");
            logger.LogError(exception.ToString());

            context.Result = new ObjectResult(Result.Exception(exception.Message, exception.ToString()))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}

