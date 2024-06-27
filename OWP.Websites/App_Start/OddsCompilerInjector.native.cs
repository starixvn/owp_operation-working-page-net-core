
using OWP.OddsCompiler;

namespace OWP.Websites;

public static class OddsCompilerInjector
{
    public static void AddOddsCompilerService(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<ExcelExportService, ExcelExportServiceImp>();
    }
}
