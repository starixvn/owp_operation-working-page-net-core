using OWP.ProviderVerification.ProviderVerification.Agents.QueryAgents;

namespace OWP.ProviderVerification.ProviderVerification.QueryAgents;

public class QueryAgentOptionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/ProviderVerification/get-agent-option", async (ISender sender) =>
            await sender.Send(new QueryAgentOptionRequest())
       ).RequireAuthorization()
        .WithName("QueryAgentOption")
            .Produces<IEnumerable<AgentOption>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("QueryAgentOption")
            .WithDescription("QueryAgentOption");
    }
}