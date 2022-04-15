
using OWP.MatchCountLiveReport;

namespace OWP.Websites;

public static class MatchLiveReportInjector
{
  public static void AddMatchLiveReportService(this IServiceCollection services)
  {
    services.AddScoped<Repository, RepositoryImp>();
  }
}