using Fanex.NAP.Client.Helpers.Web;
using MediatR;
namespace OWP.ProviderVerification.Handlers;

public class SaveEstNoteRequest : IRequest<Unit>
{
    public int OwpMatchId { get; set; }

    public string EstNote { get; set; }
}

public class SaveEstNoteHandler : IRequestHandler<SaveEstNoteRequest, Unit>
{
    private readonly ProviderVerificationRepository repository;

    private readonly INapSession currentSession;

    public SaveEstNoteHandler(ProviderVerificationRepository repository, INapSession currentSession)
    {
        this.repository = repository;
        this.currentSession = currentSession;
    }

    public async Task<Unit> Handle(SaveEstNoteRequest request, CancellationToken cancellationToken)
    {
        await repository.SaveEstNote(request.OwpMatchId, request.EstNote, currentSession.UserId);
        return Unit.Value;
    }
}
