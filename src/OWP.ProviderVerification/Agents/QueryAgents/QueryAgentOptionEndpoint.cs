using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace OWP.ProviderVerification.Agents.QueryAgents
{
    public class QueryAgentOptionEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/ProviderVerification/get-agent-option", async (ISender sender) =>
            {
                var result = await sender.Send(new QueryAgentOptionRequest());
                return result;
            }).RequireAuthorization()
            .WithName("QueryAgentOption")
                .Produces<IEnumerable<AgentOption>>()
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithSummary("QueryAgentOption")
                .WithDescription("QueryAgentOption");
        }
    }
}