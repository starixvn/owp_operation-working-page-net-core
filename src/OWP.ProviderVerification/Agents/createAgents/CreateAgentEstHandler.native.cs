namespace OWP.ProviderVerification.Agents.createAgents;

public record CreateAgentEstRequest(int OwpMatchId, string EstAgent) : Command<Unit>;

public class CreateAgentEstHandler : CommandHandler<CreateAgentEstRequest, Unit>
{
    private readonly ProviderVerificationRepository repository;

    public CreateAgentEstHandler(ProviderVerificationRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Unit> Handle(CreateAgentEstRequest request, CancellationToken cancellationToken)
    {
        await repository.SaveEstAgent(request.OwpMatchId, request.EstAgent);
        return Unit.Value;
    }
}