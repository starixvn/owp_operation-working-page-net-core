namespace OWP.ProviderVerification.ProviderVerification.QueryProvider;

public class QueryProviderVerification : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/ProviderVerification/get-provider-verification", async (DateTime eventDate, bool onlyShowIsMismatchEvent, [FromServices]ISender sender) =>
        {
            var result = await sender.Send(new QueryProviderVerificationRequest(eventDate, onlyShowIsMismatchEvent));
            return Results.Json(result, JsonSerializerOptions.Default);
        }
       ).RequireAuthorization()
        .WithName("QueryProviderVerification")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("QueryProviderVerification")
            .WithDescription("QueryProviderVerification");
    }
}