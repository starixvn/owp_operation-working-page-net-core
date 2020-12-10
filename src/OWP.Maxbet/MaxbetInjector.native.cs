using Microsoft.Extensions.DependencyInjection;
using OWP.Maxbet.Builder;
using OWP.Maxbet.Services;

namespace OWP.Maxbet
{
    public static class MaxbetInjector
    {
        public static void InjectSyncMaxbetService(this IServiceCollection services)
        {
            services.AddScoped<TraderAssignmentBuilder, TraderAssignmentBuilderImp>();
            services.AddScoped<SpreadBuilder, SpreadBuilderImp>();
            services.AddScoped<MaxbetBuilder, MaxbetBuilderImp>();
            services.AddScoped<MaxbetService, MaxbetServiceImp>();

            services.AddScoped<SyncMaxbetReposiory, SyncMaxbetReposioryImp>();
        }
    }
}