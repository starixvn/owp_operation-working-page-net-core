using OWP.WorkingShift.SyncWorkingShift;

namespace OWP.Websites.App_Start;

public static class SyncWorkingShiftInjector
{
    public static void AddSyncWorkingShift(this IServiceCollection services)
    {
        services.AddScoped<WorkingShiftRepository, WorkingShiftRepositoryImp>();
    }
}