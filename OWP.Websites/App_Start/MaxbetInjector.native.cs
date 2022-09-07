namespace OWP.Websites;

using Maxbet;
using Maxbet.Builder;
using Maxbet.Selector;
using Maxbet.Services;

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