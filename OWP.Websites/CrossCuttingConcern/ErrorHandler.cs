using Fanex.Logging.Extensions;
using ILogger = Fanex.Logging.ILogger;

namespace OWP.Websites.CrossCuttingConcern;

public class ErrorHandler
{
    private readonly RequestDelegate next;
    private readonly ILogger logger;
    public IConfiguration Configuration { get; }

    public ErrorHandler(RequestDelegate next, ILogger logger, IConfiguration configuration)
    {
        this.next = next;
        this.logger = logger;
        Configuration = configuration;
    }

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

        var requestedWith = context.Request.Headers["X-Requested-With"];

        var isAjaxRequest = requestedWith == "XMLHttpRequest";
        if (!context.Response.HasStarted && isAjaxRequest)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        await context.Response.WriteAsync(exception.Message);

        var env = Configuration.GetSection("Fanex.NAP.Client:Environment").Value;

        if (!env.Equals("LIVE", StringComparison.CurrentCultureIgnoreCase))
        {
            await context.Response.WriteAsync(Environment.NewLine);
            await context.Response.WriteAsync(exception.StackTrace);
        }
    }
}