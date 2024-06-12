
namespace OWP.Websites.App_Start;

public static class WorkingShiftInjector
{
    public static void AddWorkingShiftService(this IServiceCollection services)
    {
        services.AddTransient<WorkingShiftColorRepository, RepositoryWorkingShiftColorImp>();
        services.AddTransient<WorkingShiftTimezoneSettingRepository, WorkingShiftTimezoneSettingRepositoryImp>();
        services.AddTransient<WorkingShiftPublishRepository, WorkingShiftPublishRepositoryImp>();
        services.AddTransient<WorkingShiftDefaultRepository, WorkingShiftDefaultRepositoryImp>();
        services.AddTransient<WorkingShiftOrderRepository, WorkingShiftOrderRepositoryImp>();
        services.AddTransient<WorkingShiftRepository, WorkingShiftRepositoryImp>();
        services.AddTransient<ModelService, ModelServiceImp>();
    }
}
