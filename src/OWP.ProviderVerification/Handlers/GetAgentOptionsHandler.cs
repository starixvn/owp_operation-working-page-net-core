
namespace OWP.ProviderVerification.Handlers;

public class GetAgentOptionsRequest : IRequest<IEnumerable<AgentOption>>
{

}

internal class GetAgentOptionsHandler : IRequestHandler<GetAgentOptionsRequest, IEnumerable<AgentOption>>
{
    private readonly ProviderVerificationRepository repository;

    public GetAgentOptionsHandler(ProviderVerificationRepository repository)
    {
        this.repository = repository;
    }

    public Task<IEnumerable<AgentOption>> Handle(GetAgentOptionsRequest request, CancellationToken cancellationToken)
        => repository.GetAgentOptions();
}
