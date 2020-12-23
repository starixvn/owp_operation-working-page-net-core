using Microsoft.Extensions.DependencyInjection;
using Submit = OWP.OtherSport.Submit.Services;

namespace OWP.Websites
{
    public static class OtherSportInjector
    {
        public static void InjectOtherSportService(this IServiceCollection services)
        {
            // Submit
            services.AddScoped<Submit.CacheManipulator, Submit.CacheManipulatorImp>();
            services.AddScoped<Submit.AssignmentAnalyzer, Submit.AssignmentAnalyzerImp>();
            services.AddScoped<Submit.AssignmentMaxbetMapper, Submit.AssignmentMaxbetMapperImp>();
            services.AddScoped<Submit.CommandRepository, Submit.CommandRepositoryImp>();
        }
    }
}