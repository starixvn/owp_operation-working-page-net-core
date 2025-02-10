namespace OWP.ProviderVerification.ProviderVerification.QueryProvider;

public record QueryProviderVerificationRequest(DateTime EventDate, bool OnlyShowIsMismatchEvent) : Query<IEnumerable<ProviderVerificationModel>>;
public class QueryProviderVerificationHandler : QueryHandler<QueryProviderVerificationRequest, IEnumerable<ProviderVerificationModel>>
{
    private readonly ProviderVerificationRepository repository;

    public QueryProviderVerificationHandler(ProviderVerificationRepository repository)
    {
        this.repository = repository;
    }


    public async Task<IEnumerable<ProviderVerificationModel>> Handle(QueryProviderVerificationRequest request, CancellationToken cancellationToken)
    {
        var criteria = new GetProviderVerificationCriteria
        {
            EventDate = request.EventDate,
            OnlyShowIsMismatchEvent = request.OnlyShowIsMismatchEvent
        };

        return await repository.GetProviderVerification(criteria);
    }
}