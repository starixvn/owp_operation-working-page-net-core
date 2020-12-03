using Microsoft.Extensions.DependencyInjection;
using OWP.API.SyncMaxbet.service;

namespace OWP.API.SyncMaxbet
{
    public static class SyncMaxbetServiceCollectionExtensions
    {
        public static void ConfigMaxbetService(this IServiceCollection services)
        {
            services.AddScoped<MaxbetService, MaxbetServiceImp>();
            services.AddScoped<SyncMaxbetReposiory,SyncMaxbetReposioryImp>();
        }
    }
}
