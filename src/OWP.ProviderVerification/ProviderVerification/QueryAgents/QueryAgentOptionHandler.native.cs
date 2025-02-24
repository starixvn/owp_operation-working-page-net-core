namespace OWP.ProviderVerification.ProviderVerification.QueryAgents;

public record QueryAgentOptionRequest : Query<IEnumerable<AgentOption>>;

public class GetAgentOptionsHandler(ProviderVerificationRepository repository)
    : QueryHandler<QueryAgentOptionRequest, IEnumerable<AgentOption>>
{
    public Task<IEnumerable<AgentOption>> Handle(QueryAgentOptionRequest request, CancellationToken cancellationToken)
    => repository.GetAgentOptions();
}