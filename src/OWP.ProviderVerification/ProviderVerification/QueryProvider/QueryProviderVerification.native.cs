
namespace OWP.ProviderVerification.ProviderVerification.QueryProvider;

public record QueryProviderVerificationRequest(DateTime EventDate, bool OnlyShowIsMismatchEvent, string ConfirmProvider, string LstBookProvider, string SessionId) : Query<ResultT<AnalyzedProviderResult>>;
public class QueryProviderVerificationHandler(ICache cache, ProviderVerificationRepository repository)
    : QueryHandler<QueryProviderVerificationRequest, ResultT<AnalyzedProviderResult>>
{
    private const int CACHE_EXPIRED_IN_DAYS = 1;

    public async Task<ResultT<AnalyzedProviderResult>> Handle(QueryProviderVerificationRequest request, CancellationToken cancellationToken)
    {
        var criteria = new GetProviderVerificationCriteria
        {
            EventDate = request.EventDate,
            OnlyShowIsMismatchEvent = request.OnlyShowIsMismatchEvent,
            ConfirmProvider = request.ConfirmProvider,
            LSTBookProvider = request.LstBookProvider
        };

        var providerVerification = await repository.GetProviderVerification(criteria);

        if (providerVerification.Any())
        {
            await SaveProviderVerificationInfo(request.SessionId, providerVerification.ToList());
        }

        return ResultT<AnalyzedProviderResult>.Success(new AnalyzedProviderResult(request.SessionId, providerVerification.ToList()));
    }

    private async Task SaveProviderVerificationInfo(string sessionId, IList<ProviderVerificationModel> providerModel)
    {
        var expiredDate = DateTime.Today.AddDays(CACHE_EXPIRED_IN_DAYS);
        var absoluteTimeSpan = expiredDate - DateTime.Now;
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(absoluteTimeSpan);
        var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
        var assignmentsJson = JsonConvert.SerializeObject(providerModel);
        await cache.SetAsync(cacheKey, assignmentsJson, cachePolicy);
    }
}