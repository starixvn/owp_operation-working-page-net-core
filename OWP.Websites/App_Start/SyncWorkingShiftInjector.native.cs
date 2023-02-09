namespace OWP.Websites;

using ApiSyncWorkingShift;

public static class SyncWorkingShiftInjector
{
  public static void AddSyncWorkingShift(this IServiceCollection services)
  {
    services.AddScoped<WorkingShiftRepository, WorkingShiftRepositoryImp>();
  }
}