
using OWP.DailyWorkingShift.Repositories;

namespace OWP.Websites.App_Start;

public static class WorkingShiftInjector
{
    public static void AddDailyWorkingShiftService(this IServiceCollection services)
    {
       
        services.AddTransient<Repository, RepositoryImp>();
        services.AddTransient<DailyWorkingShift.Services.ModelService, DailyWorkingShift.Services.ModelServiceImp>();
    }
}
