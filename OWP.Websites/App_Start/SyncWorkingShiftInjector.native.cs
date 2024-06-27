using OWP.WorkingShift.SyncWorkingShift;

namespace OWP.Websites;

public static class SyncWorkingShiftInjector
{
    public static void AddSyncWorkingShift(this IServiceCollection services)
    {
        services.AddScoped<WorkingShiftRepository, WorkingShiftRepositoryImp>();
    }
}