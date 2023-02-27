using OWP.ProductAssignmentReference;
using OWP.ProductAssignmentReference.Services;

namespace OWP.Websites;

public static class ProductAssignmentInjector
{
  public static void AddProductAssignment(this IServiceCollection services)
  {
    services.AddScoped<ProductAssignmentRepository, ProductAssignmentRepositoryImp>();
    services.AddScoped<ProductAssignmentService, ProductAssignmentServiceImp>();
  }
}