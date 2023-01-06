using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OWP.Middleware;

public abstract class AsyncFilter<T> : IAsyncActionFilter where T : class
{
    public abstract (bool, int, string) Execute(T model);

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var param = context
        .ActionArguments
        .FirstOrDefault(p => p.Value is T)
        .Value;

        var type = typeof(T);
        if (param is T model)
        {
            var (canNext, statusCode, contentResult) = Execute(model);
            if (!canNext)
            {
                context.HttpContext.Response.StatusCode = statusCode;
                context.Result = new ContentResult
                {
                    Content = contentResult
                };
                return;
            }
        }
        await next();
    }
}
