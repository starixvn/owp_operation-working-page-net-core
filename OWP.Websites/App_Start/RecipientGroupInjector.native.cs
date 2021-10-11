namespace OWP.Websites.App_Start
{
    using Microsoft.Extensions.DependencyInjection;
    using OWP.RecipientGroup.Services;

    public static class RecipientGroupInjector
    {
        public static void InjectRecipientGroup(this IServiceCollection services)
        {
            services.AddScoped<CommandRepository, CommandRepositoryImp>();
        }
    }
}