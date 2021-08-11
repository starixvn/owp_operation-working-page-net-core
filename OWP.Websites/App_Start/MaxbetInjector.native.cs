using Microsoft.Extensions.DependencyInjection;
using OWP.Maxbet;
using OWP.Maxbet.Builder;
using OWP.Maxbet.Selector;
using OWP.Maxbet.Services;

namespace OWP.Websites
{
    public static class MaxbetInjector
    {
        public static void InjectSyncMaxbetService(this IServiceCollection services)
        {
            services.AddScoped<TraderAssignmentBuilder, TraderAssignmentBuilderImp>();
            services.AddScoped<SpreadBuilder, SpreadBuilderImp>();
            services.AddScoped<MaxbetBuilder, MaxbetBuilderImp>();
            services.AddScoped<MaxbetService, MaxbetServiceImp>();

            services.AddScoped<SpecialProductMaxbetBuilder, SpecialProductMaxbetBuilderImp>();
            services.AddScoped<SpecialProductSpreadBuilder, SpecialProductSpreadBuilderImp>();
            services.AddScoped<SpecialProductTraderBuilder, SpecialProductTraderBuilderImp>();
            services.AddScoped<SpecialProductSelector, SpecialProductSelectorImp>();


            services.AddScoped<MaxbetReposiory, MaxbetReposioryImp>();
        }
    }
}