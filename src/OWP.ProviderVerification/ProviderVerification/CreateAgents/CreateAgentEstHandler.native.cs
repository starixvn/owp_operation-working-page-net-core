namespace OWP.ProviderVerification.ProviderVerification.createAgents;

public record CreateAgentEstRequest(int OwpMatchId, string EstAgent) : Command<Unit>;

public class CreateAgentEstHandler(ProviderVerificationRepository repository) : CommandHandler<CreateAgentEstRequest, Unit>
{
    
    public async Task<Unit> Handle(CreateAgentEstRequest request, CancellationToken cancellationToken)
    {
        await repository.SaveEstAgent(request.OwpMatchId, request.EstAgent);
        return Unit.Value;
    }
}