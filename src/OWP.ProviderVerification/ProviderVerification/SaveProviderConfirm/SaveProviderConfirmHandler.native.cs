

namespace OWP.ProviderVerification.ProviderVerification.SaveProviderConfirm;

public record SaveProviderConfirmRequest(int OwpMatchId, string AgentSelected) : Command<Unit>;

public class SaveProviderConfirmHandler(ProviderVerificationRepository repository, INapSession currentSession) : CommandHandler<SaveProviderConfirmRequest, Unit>
{
    public async Task<Unit> Handle(SaveProviderConfirmRequest request, CancellationToken cancellationToken)
    {
        await repository.SaveAgentConfirm(request.OwpMatchId, request.AgentSelected, currentSession.UserId);
        return Unit.Value;
    }
}



