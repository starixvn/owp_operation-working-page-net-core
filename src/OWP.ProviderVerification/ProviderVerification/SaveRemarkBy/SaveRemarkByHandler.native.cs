namespace OWP.ProviderVerification.ProviderVerification.SaveRemarkBy;

public class SaveRemarkByRequest : IRequest<Unit>
{
    public int OwpMatchId { get; set; }

    public string RemarkBy { get; set; }
}

public class SaveRemarkByHandler : IRequestHandler<SaveRemarkByRequest, Unit>
{
    private readonly ProviderVerificationRepository repository;

    public SaveRemarkByHandler(ProviderVerificationRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Unit> Handle(SaveRemarkByRequest request, CancellationToken cancellationToken)
    {
        await repository.SaveRemark(request.OwpMatchId, request.RemarkBy);
        return Unit.Value;
    }
}