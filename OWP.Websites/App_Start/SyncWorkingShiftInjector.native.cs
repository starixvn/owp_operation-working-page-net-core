namespace OWP.Websites.App_Start;

public static class SyncWorkingShiftInjector
{
    public static void AddSyncWorkingShift(this IServiceCollection services)
    {
        services.AddScoped<ApiSyncWorkingShift.WorkingShiftRepository, ApiSyncWorkingShift.WorkingShiftRepositoryImp>();
    }
}