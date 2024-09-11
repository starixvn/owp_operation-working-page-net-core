using OWP.Api.Maxbet;
using OWP.Api.Maxbet.Builder;
using OWP.Api.Maxbet.Selector;
using OWP.Api.Maxbet.Services;
namespace OWP.Websites;

public static class MaxbetInjector
{
    public static void AddSyncMaxbet(this IServiceCollection services)
    {
        services.AddScoped<TraderAssignmentBuilder, TraderAssignmentBuilderImp>();
        services.AddScoped<SpreadBuilder, SpreadBuilderImp>();
        services.AddScoped<MaxbetBuilder, MaxbetBuilderImp>();
        services.AddScoped<MainMatchService, MainMatchServiceImp>();
        services.AddScoped<SpecialProductService, SpecialProductServiceImp>();
        services.AddScoped<SabaMatchService, SabaMatchServiceImp>();
        services.AddScoped<FutsalMatchService, FutsalMatchServiceImp>();
        services.AddScoped<SpecialProductMaxbetBuilder, SpecialProductMaxbetBuilderImp>();
        services.AddScoped<SpecialProductSpreadBuilder, SpecialProductSpreadBuilderImp>();
        services.AddScoped<SpecialProductTraderBuilder, SpecialProductTraderBuilderImp>();
        services.AddScoped<SpecialProductSelector, SpecialProductSelectorImp>();
        services.AddScoped<ServiceCommon, ServiceCommonImp>();
        services.AddScoped<ETPKMatchService, ETPKMatchServiceImp>();
        services.AddScoped<OddService, OddServiceImp>();
        services.AddScoped<ServiceCommon, ServiceCommonImp>();

        services.AddScoped<MaxbetRepository, MaxbetRepositoryImp>();
    }
}