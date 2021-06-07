using Microsoft.Extensions.DependencyInjection;
using OWP.ApiSyncWorkingShift;

namespace OWP.Websites.App_Start
{
    public static class SyncWorkingShiftInjector
    {
        public static void InjectSyncWorkingShift(this IServiceCollection services)
        {
            services.AddScoped<WorkingShiftRepository, WorkingShiftRepositoryImp>();
        }
    }
}
