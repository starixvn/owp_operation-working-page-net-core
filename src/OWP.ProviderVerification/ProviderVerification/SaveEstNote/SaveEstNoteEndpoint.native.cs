
namespace OWP.ProviderVerification.ProviderVerification.SaveAgentConfirm;

public class SaveEstNoteEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/ProviderVerification/save-est-note", async (SaveEstNoteRequest request, [FromServices]ISender sender) =>
        {
                    var result = await sender.Send(request);
            return Results.Json(result, JsonSerializerOptions.Default);
                }
            ).RequireAuthorization()
            .WithName("SaveEstNote")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("SaveEstNote")
            .WithDescription("SaveEstNote");
    }
}
