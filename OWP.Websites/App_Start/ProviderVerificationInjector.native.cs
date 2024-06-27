using OWP.ProviderVerification;

namespace OWP.Websites;

public static class ProviderVerificationInjector
{
    public static void AddProviderVerificationService(this IServiceCollection services)
    {
        services.AddScoped<ProviderVerificationRepository, ProviderVerificationRepositoryImp>();
    }
}
