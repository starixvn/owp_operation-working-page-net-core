namespace OWP.ProviderVerification.ProviderVerification.QueryLSTBookProvider;

public class QueryLSTBookProviderOptionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/ProviderVerification/get-lst-book-option", async (DateTime eventDate, ISender sender) =>
            await sender.Send(new QueryLSTBookProviderOptionRequest(eventDate))
       ).RequireAuthorization()
        .WithName("QueryLSTBookOption")
            .Produces<IEnumerable<LSTBookProviderOption>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("QueryLSTBookOption")
            .WithDescription("QueryLSTBookOption");
    }
}