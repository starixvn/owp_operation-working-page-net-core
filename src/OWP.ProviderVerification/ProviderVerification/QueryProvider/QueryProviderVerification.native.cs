namespace OWP.ProviderVerification.ProviderVerification.QueryProvider;

public record QueryProviderVerificationRequest(DateTime EventDate, bool OnlyShowIsMismatchEvent, string ConfirmProvider) : Query<IEnumerable<ProviderVerificationModel>>;
public class QueryProviderVerificationHandler(ProviderVerificationRepository repository)
    : QueryHandler<QueryProviderVerificationRequest, IEnumerable<ProviderVerificationModel>>
{
    public async Task<IEnumerable<ProviderVerificationModel>> Handle(QueryProviderVerificationRequest request, CancellationToken cancellationToken)
    {
        var criteria = new GetProviderVerificationCriteria
        {
            EventDate = request.EventDate,
            OnlyShowIsMismatchEvent = request.OnlyShowIsMismatchEvent,
            ConfirmProvider = request.ConfirmProvider
        };

        return await repository.GetProviderVerification(criteria);
    }
}