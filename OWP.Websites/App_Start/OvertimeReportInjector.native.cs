namespace OWP.Websites.App_Start
{
    using Microsoft.Extensions.DependencyInjection;
    using OWP.OvertimeReport.Services;

    public static class OvertimeReportInjector
    {
        public static void InjectOvertimeReport(this IServiceCollection services)
        {
            services.AddScoped<CommandRepository, CommandRepositoryImp>();
        }
    }
}