using OWP.RecipientGroup.Services;

namespace OWP.Websites;

public static class RecipientGroupInjector
{
    public static void AddRecipientGroup(this IServiceCollection services)
    {
        services.AddScoped<CommandRepository, CommandRepositoryImp>();
    }
}