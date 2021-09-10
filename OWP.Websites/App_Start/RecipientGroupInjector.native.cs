namespace OWP.Websites.App_Start
{
    using Microsoft.Extensions.DependencyInjection;

    public static class RecipientGroupInjector
    {
        public static void InjectRecipientGroup(this IServiceCollection services)
        {
            //services.AddScoped<CommandRepository, CommandRepositoryImp>();
        }
    }
}