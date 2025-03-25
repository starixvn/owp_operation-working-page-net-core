

using Microsoft.AspNetCore.Hosting;

namespace OWP.ProviderVerification.ProviderVerification.DownloadTemplate;
public class DownloadTemplateEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/ProviderVerification/get-template/ProviderVerificationUploadTemplate.xlsx", ([FromServices] IWebHostEnvironment hostEnvironment) =>
                {
                    const string FILE_NAME = "ProviderVerificationUploadTemplate.xlsx";
                    var templatePath = Path.Combine(hostEnvironment.ContentRootPath, "App_Data", "Templates", FILE_NAME);
                    using var fileBytes = File.ReadAllBytesAsync(templatePath);
                    return Results.File(fileBytes.Result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FILE_NAME);
                }
            ).RequireAuthorization()
            .WithName("GetTemplate/ProviderVerificationUploadTemplate")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("GetTemplate/ProviderVerificationUploadTemplate")
            .WithDescription("GetTemplate/ProviderVerificationUploadTemplate");
    }
}
