namespace OWP.ProviderVerification.ProviderVerification.QueryLSTBookProvider;

public record QueryLSTBookProviderOptionRequest(DateTime EventDate) : Query<IEnumerable<LSTBookProviderOption>>;

public class QueryLSTBookProviderOptionHandler(ProviderVerificationRepository repository)
    : QueryHandler<QueryLSTBookProviderOptionRequest, IEnumerable<LSTBookProviderOption>>
{
    public Task<IEnumerable<LSTBookProviderOption>> Handle(QueryLSTBookProviderOptionRequest request, CancellationToken cancellationToken)
    => repository.GetLSTBookOptions(request.EventDate);
}