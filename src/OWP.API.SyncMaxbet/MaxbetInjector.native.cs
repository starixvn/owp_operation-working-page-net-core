using Microsoft.Extensions.DependencyInjection;
using OWP.API.SyncMaxbet.Builder;
using OWP.API.SyncMaxbet.Services;

namespace OWP.API.SyncMaxbet
{
    public static class MaxbetInjector
    {
        public static void InjectSyncMaxbetService(this IServiceCollection services)
        {
            services.AddScoped<TraderAssignmentBuilder, TraderAssignmentBuilderImp>();
            services.AddScoped<SpreadBuilder, SpreadBuilderImp>();
            services.AddScoped<MaxbetBuilder, MaxbetBuilderImp>();
            services.AddScoped<MaxbetService, MaxbetServiceImp>();
            
            services.AddScoped<SyncMaxbetReposiory,SyncMaxbetReposioryImp>();
        }
    }
}
