
namespace OWP.Websites;

using MatchCountLiveReport;

public static class MatchLiveReportInjector
{
  public static void AddMatchLiveReportService(this IServiceCollection services)
  {
    services.AddScoped<Repository, RepositoryImp>();
  }
}