namespace OWP.ProviderVerification.ProviderVerification.QueryProvider;

public record QueryProviderVerificationRequest(DateTime EventDate, bool IsMismatchEvent) : Query<IEnumerable<ProviderVerificationModel>>;
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
            IsMismatchEvent = request.IsMismatchEvent
        };

        return await repository.GetProviderVerification(criteria);
    }
}