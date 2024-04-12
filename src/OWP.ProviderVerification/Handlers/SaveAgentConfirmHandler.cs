
using Fanex.NAP.Client.Helpers.Web;
using MediatR;

namespace OWP.ProviderVerification.Handlers;

public class SaveAgentConfirmRequest : IRequest<Unit>
{
    public int OwpMatchId { get; set; }

    public string AgentSelected { get; set; }
}

public class SaveAgentConfirmHandler : IRequestHandler<SaveAgentConfirmRequest,Unit>
{
    private readonly ProviderVerificationRepository repository;

    private readonly INapSession currentSession;

    public SaveAgentConfirmHandler(ProviderVerificationRepository repository, INapSession currentSession)
    {
        this.repository = repository;
        this.currentSession = currentSession;
    }

    public async Task<Unit> Handle(SaveAgentConfirmRequest request, CancellationToken cancellationToken)
    {
        await repository.SaveAgentConfirm(request.OwpMatchId, request.AgentSelected, currentSession.UserId);
        return Unit.Value;
    }
}
