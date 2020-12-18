using Microsoft.Extensions.DependencyInjection;
using OWP.OtherSport.Submit.Services;

namespace OWP.Websites
{
    public static class OtherSportInjector
    {
        public static void InjectOtherSportService(this IServiceCollection services)
        {
            services.AddScoped<CacheManipulator, CacheManipulatorImp>();
        }
    }
}