namespace OWP.ProviderVerification.Agents.QueryAgents;

public record QueryAgentOptionRequest() : Query<IEnumerable<AgentOption>>;

internal class GetAgentOptionsHandler : QueryHandler<QueryAgentOptionRequest, IEnumerable<AgentOption>>
{
    private readonly ProviderVerificationRepository repository;

    public GetAgentOptionsHandler(ProviderVerificationRepository repository)
    {
        this.repository = repository;
    }

    public Task<IEnumerable<AgentOption>> Handle(QueryAgentOptionRequest request, CancellationToken cancellationToken)
    => repository.GetAgentOptions();
}