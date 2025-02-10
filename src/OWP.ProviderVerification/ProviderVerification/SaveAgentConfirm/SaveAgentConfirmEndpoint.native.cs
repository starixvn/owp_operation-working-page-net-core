
namespace OWP.ProviderVerification.ProviderVerification.SaveAgentConfirm;

internal class SaveAgentConfirmEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/ProviderVerification/save-agent-confirm", async (SaveAgentConfirmRequest request, ISender sender) =>
        {
                    var result = await sender.Send(request);
            return Results.Json(result, JsonSerializerOptions.Default);
                }
            ).RequireAuthorization()
            .WithName("SaveAgentConfirm")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("SaveAgentConfirm")
            .WithDescription("SaveAgentConfirm");
    }
}
