using OWP.WorkingShift.DailyWorkingShift.Services;
using Repositories = OWP.WorkingShift.DailyWorkingShift.Repositories;

namespace OWP.Websites.App_Start;

public static class WorkingShiftInjector
{
    public static void AddDailyWorkingShiftService(this IServiceCollection services)
    {
       
        services.AddTransient<Repositories.Repository, Repositories.RepositoryImp>();
        services.AddTransient<ModelService, ModelServiceImp>();
    }
}
