
namespace OWP.ProviderVerification.ProviderVerification.UploadAgent;

public record SubmitAgentRequest(string SessionId) : Query<ResultT<IList<ProviderVerificationModel>>>;

public class SubmitAgentHandler(ICache cache, ProviderVerificationRepository repository) : QueryHandler<SubmitAgentRequest, ResultT<IList<ProviderVerificationModel>>>
{
    private const int CACHE_EXPIRED_IN_DAYS = 1;

    public async Task<ResultT<IList<ProviderVerificationModel>>> Handle(SubmitAgentRequest request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{Const.Const.KEY_PROVIDER_VERIFICATION}-{request.SessionId}";

        var (_, excelProviderModel) = await cache.TryGetAsync<string>(cacheKey);
        var excelProviderModels = JsonConvert.DeserializeObject<IList<ExcelProviderModel>>(excelProviderModel);
        var compareResult = await Compare(excelProviderModels);
        if (compareResult?.Any() == true)
        {
            await SaveProviderCompareInfo(request.SessionId, compareResult);
        }

        return ResultT<IList<ProviderVerificationModel>>.Success(compareResult);
    }

    private async Task<IList<ProviderVerificationModel>> Compare(IList<ExcelProviderModel> excelProviderModels)
    {
        var criteria = new GetProviderVerificationCriteria
        {
            EventDate = new DateTime(2025, 2, 12),
            OnlyShowIsMismatchEvent = false
        };

        var providerVerification = await repository.GetProviderVerification(criteria);

        var excelProviderMatchIds = new HashSet<string>(excelProviderModels.Select(e => e.ProviderMatchID));

        var unmatchedInProviderVerification = providerVerification
            .Where(pv => pv.ProviderMatch.All(pm => !excelProviderMatchIds.Contains(pm.ProviderMatchId.ToString())))
            .ToList();

        var unmatchedInExcel = excelProviderModels
            .Where(e => !excelProviderMatchIds.Contains(e.ProviderMatchID))
            .Select(e => new ProviderVerificationModel
            {
                OWPMatchId = 0,
                MatchNo = string.Empty,
                MatchStatus = string.Empty,
                EventDateTime = DateTime.MinValue,
                KoTime = string.Empty,
                League = string.Empty,
                Home = string.Empty,
                Away = string.Empty,
                LstBookProvider = string.Empty,
                InHouseProviderId = string.Empty,
                AgentEst = string.Empty,
                RemarkEst = string.Empty,
                RemarkBy = string.Empty,
                AgentConfirm = new List<AgentConfirmOption>(),
                Provider = e.Provider,
                ProviderMatchId = e.ProviderMatchID
            })
            .ToList();

        unmatchedInProviderVerification.AddRange(unmatchedInExcel);

        return unmatchedInProviderVerification;
    }
    private async Task SaveProviderCompareInfo(string sessionId, IList<ProviderVerificationModel> providerVerification)
    {
        var expiredDate = DateTime.Today.AddDays(CACHE_EXPIRED_IN_DAYS);
        var absoluteTimeSpan = expiredDate - DateTime.Now;
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(absoluteTimeSpan);
        var cacheKey = $"{Const.Const.KEY_PROVIDER_COMPARE_VERIFICATION}-{sessionId}";
        var assignmentsJson = JsonConvert.SerializeObject(providerVerification);
        await cache.SetAsync(cacheKey, assignmentsJson, cachePolicy);
    }
}
