using OWP.RecipientGroup.Services;

namespace OWP.Websites.App_Start;

public static class RecipientGroupInjector
{
    public static void AddRecipientGroup(this IServiceCollection services)
    {
        services.AddScoped<CommandRepository, CommandRepositoryImp>();
    }
}