using Fanex.Logging.Extensions;
using ILogger = Fanex.Logging.ILogger;

namespace OWP.Websites.CrossCuttingConcern;

public class ErrorHandler(RequestDelegate next, ILogger logger, IConfiguration configuration)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleException(context, exception);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        var logMessage = new LogMessage(exception.Message)
            .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
            .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
            .ToString();
        await logger.ErrorAsync(logMessage);

        // If the response has already started, we can't modify headers or write to it
        if (context.Response.HasStarted)
        {
            return;
        }

        var requestedWith = context.Request.Headers["X-Requested-With"];

        var isAjaxRequest = requestedWith == "XMLHttpRequest";
        if (isAjaxRequest)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        await context.Response.WriteAsync(exception.Message);

        var env = configuration.GetSection("Fanex.NAP.Client:Environment").Value;

        if (!env.Equals("LIVE", StringComparison.CurrentCultureIgnoreCase))
        {
            await context.Response.WriteAsync(Environment.NewLine);
            await context.Response.WriteAsync(exception.StackTrace);
        }
    }
}