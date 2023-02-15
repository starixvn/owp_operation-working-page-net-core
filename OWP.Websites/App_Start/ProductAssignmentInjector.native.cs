namespace OWP.Websites;

using ProductAssignmentReference;
using ProductAssignmentReference.Services;

public static class ProductAssignmentInjector
{
  public static void AddProductAssignment(this IServiceCollection services)
  {
    services.AddScoped<ProductAssignmentRepository, ProductAssignmentRepositoryImp>();
    services.AddScoped<ProductAssignmentService, ProductAssignmentServiceImp>();
  }
}