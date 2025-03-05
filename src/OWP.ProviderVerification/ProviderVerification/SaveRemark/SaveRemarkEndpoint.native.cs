using OWP.ProviderVerification.ProviderVerification.SaveRemark;

public class SaveRemarkEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/ProviderVerification/save-remark", async (SaveRemarkRequest request, [FromServices] ISender sender) =>
        {
            var result = await sender.Send(request);
            return Results.Json(result, JsonSerializerOptions.Default);
        }
            ).RequireAuthorization()
            .WithName("SaveRemark")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("SaveRemark")
            .WithDescription("SaveRemark");
    }
}
