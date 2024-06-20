
using OWP.OddsCompiler;
using OWP.Services;

namespace OWP.Websites.App_Start;

public static class OddsCompilerInjector
{
    public static void AddOddsCompilerService(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<ExcelExportService, ExcelExportServiceImp>();
    }
}
