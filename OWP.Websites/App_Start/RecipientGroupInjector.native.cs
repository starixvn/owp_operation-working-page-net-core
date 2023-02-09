namespace OWP.Websites;

using RecipientGroup.Services;

public static class RecipientGroupInjector
{
  public static void AddRecipientGroup(this IServiceCollection services)
  {
    services.AddScoped<CommandRepository, CommandRepositoryImp>();
  }
}