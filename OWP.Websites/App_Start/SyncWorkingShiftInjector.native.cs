
namespace OWP.Websites;

public static class SyncWorkingShiftInjector
{
  public static void AddSyncWorkingShift(this IServiceCollection services)
  {
    services.AddScoped<ApiSyncWorkingShift.WorkingShiftRepository, ApiSyncWorkingShift.WorkingShiftRepositoryImp>();
  }
}