using OWP.OvertimeReport.Services;

namespace OWP.Websites;

public static class OvertimeReportInjector
{
  public static void AddOvertimeReport(this IServiceCollection services)
  {
    services.AddScoped<CommandRepository, CommandRepositoryImp>();
    services.AddScoped<ReportByMonthServices, ReportByMonthServicesImp>();
    services.AddScoped<ExportReportByMonthServices, ExportReportByMonthServicesImp>();
  }
}