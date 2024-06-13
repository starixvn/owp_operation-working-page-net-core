
using OWP.OddsCompiler;

namespace OWP.Websites.App_Start;

public static class OddsCompilerInjector
{
    public static void AddOddsCompilerService(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<ExcelExportService, ExcelExportServiceImp>();
        services.AddScoped<NotificationOddCompilerService, NotificationOddCompilerServiceImp>();
    }
}
