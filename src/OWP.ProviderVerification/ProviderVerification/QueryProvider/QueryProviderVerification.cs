namespace OWP.ProviderVerification.Provider;

public record QueryProviderVerificationRequest(DateTime EventDate) : Query<IEnumerable<ProviderVerificationModel>>;
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
            EventDate = request.EventDate
        };

        return await repository.GetProviderVerification(criteria);
    }
}