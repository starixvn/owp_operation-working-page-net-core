


namespace OWP.ProviderVerification.Handlers;

public class GetProviderVerificationRequest : IRequest<IEnumerable<ProviderVerificationModel>>
{
    public DateTime EventDate;
}
public class GetProviderVerificationHandler : IRequestHandler<GetProviderVerificationRequest, IEnumerable<ProviderVerificationModel>>
{
    private readonly ProviderVerificationRepository repository;

    public GetProviderVerificationHandler(ProviderVerificationRepository repository)
    {
        this.repository = repository;
    }
    public async Task<IEnumerable<ProviderVerificationModel>> Handle(GetProviderVerificationRequest request, CancellationToken cancellationToken)
    {
        var criteria = new GetProviderVerificationCriteria
        {
            EventDate = request.EventDate
        };

        return await repository.GetProviderVerification(criteria);
    }
}
