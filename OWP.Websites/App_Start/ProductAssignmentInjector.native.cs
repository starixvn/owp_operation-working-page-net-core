using Microsoft.Extensions.DependencyInjection;
using OWP.ProductAssignmentReference;
using OWP.ProductAssignmentReference.Services;

namespace OWP.Websites.App_Start
{
    public static class ProductAssignmentInjector
    {
        public static void InjectProductAssignment(this IServiceCollection services)
        {
            services.AddScoped<ProductAssignmentRepository, ProductAssignmentRepositoryImp>();
            services.AddScoped<ProductAssignmentService, ProductAssignmentServiceImp>();
        }
    }
}
