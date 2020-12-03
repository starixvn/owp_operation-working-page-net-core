using Microsoft.Extensions.DependencyInjection;
using OWP.API.SyncMaxbet.Services;

namespace OWP.API.SyncMaxbet
{
    public static class MaxbetInjector
    {
        public static void InjectSyncMaxbetService(this IServiceCollection services)
        {
            services.AddScoped<MaxbetService, MaxbetServiceImp>();
            services.AddScoped<SyncMaxbetReposiory,SyncMaxbetReposioryImp>();
        }
    }
}
