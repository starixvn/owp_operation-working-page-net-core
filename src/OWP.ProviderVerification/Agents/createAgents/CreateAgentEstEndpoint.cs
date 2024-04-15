
namespace OWP.ProviderVerification.Agents.QueryAgents;

public class CreateAgentEstEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/ProviderVerification/create-est-agent", async (CreateAgentEstRequest request, ISender sender) =>
            await sender.Send(request)
       ).RequireAuthorization()
        .WithName("CreateEstAgent")
            .Produces<Unit>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("CreateEstAgent")
            .WithDescription("CreateEstAgent");
    }
}