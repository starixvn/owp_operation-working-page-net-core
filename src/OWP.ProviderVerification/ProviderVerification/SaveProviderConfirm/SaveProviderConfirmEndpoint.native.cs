
using OWP.ProviderVerification.ProviderVerification.SaveProviderConfirm;

namespace OWP.ProviderVerification.ProviderVerification.SaveAgentConfirm;

public class SaveProviderConfirmEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/ProviderVerification/save-provider-confirm", async (SaveProviderConfirmRequest request, [FromServices]ISender sender) =>
        {
                    var result = await sender.Send(request);
            return Results.Json(result, JsonSerializerOptions.Default);
                }
            ).RequireAuthorization()
            .WithName("SaveProviderConfirm")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("SaveProviderConfirm")
            .WithDescription("SaveProviderConfirm");
    }
}
