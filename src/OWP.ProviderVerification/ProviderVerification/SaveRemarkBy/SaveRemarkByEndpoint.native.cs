using OWP.ProviderVerification.ProviderVerification.SaveRemarkBy;

public class SaveRemarkByEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/ProviderVerification/save-remark-by", async (SaveRemarkByRequest request, [FromServices] ISender sender) =>
        {
            var result = await sender.Send(request);
            return Results.Json(result, JsonSerializerOptions.Default);
        }
            ).RequireAuthorization()
            .WithName("SaveRemarkByConfirm")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("SaveRemarkByConfirm")
            .WithDescription("SaveRemarkByConfirm");
    }
}
