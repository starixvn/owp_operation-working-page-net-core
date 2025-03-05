namespace OWP.ProviderVerification.ProviderVerification.SaveRemark;

public class SaveRemarkRequest : IRequest<Unit>
{
    public int OwpMatchId { get; set; }

    public string Remark { get; set; }
}

public class SaveEstNoteHandler : IRequestHandler<SaveRemarkRequest, Unit>
{
    private readonly ProviderVerificationRepository repository;

    private readonly INapSession currentSession;

    public SaveEstNoteHandler(ProviderVerificationRepository repository, INapSession currentSession)
    {
        this.repository = repository;
        this.currentSession = currentSession;
    }

    public async Task<Unit> Handle(SaveRemarkRequest request, CancellationToken cancellationToken)
    {
        await repository.SaveRemark(request.OwpMatchId, request.Remark);
        return Unit.Value;
    }
}