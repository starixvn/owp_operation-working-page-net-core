
namespace OWP.ProviderVerification.ProviderVerification.UploadAgent;

public record SubmitAgentRequest(string SessionId, DateTime Date) : Query<ResultT<IList<ProviderVerificationDifferenceModel>>>;

public class SubmitAgentHandler(ICache cache, ProviderVerificationRepository repository) : QueryHandler<SubmitAgentRequest, ResultT<IList<ProviderVerificationDifferenceModel>>>
{
    private const int CACHE_EXPIRED_IN_DAYS = 1;

    public async Task<ResultT<IList<ProviderVerificationDifferenceModel>>> Handle(SubmitAgentRequest request, CancellationToken cancellationToken)
    {
        var cacheKey = Utils.BuildProviderVerificationKey(request.SessionId);
        var (_, excelProviderModel) = await cache.TryGetAsync<string>(cacheKey);
        var excelProviderModels = JsonConvert.DeserializeObject<IList<ExcelProviderModel>>(excelProviderModel);
        var compareResult = await Compare(request.Date, excelProviderModels);

        return ResultT<IList<ProviderVerificationDifferenceModel>>.Success(compareResult);
    }

    private async Task<IList<ProviderVerificationDifferenceModel>> Compare(DateTime date, IList<ExcelProviderModel> excelProviderModels)
    {
        var criteria = new GetProviderVerificationCriteria
        {
            EventDate = date,
            OnlyShowIsMismatchEvent = false
        };

        var providerVerification = await repository.GetProviderVerificationDiff(criteria);

        var excelProviderMatchIDs = excelProviderModels.Select(e => e.ProviderMatchID).ToHashSet();

        var unMapProviderVerification = providerVerification
            .Where(pv => pv.ProviderMatch == null || pv.ProviderMatch.Count == 0 ||
                         !pv.ProviderMatch.All(pm => excelProviderMatchIDs.Contains(pm.ProviderMatchId.ToString())))
            .ToList();

        if (unMapProviderVerification != null)
        {
            string OwpMatchIds = string.Join(", ",
                unMapProviderVerification?
                    .Where(pv => pv.OWPMatchId.HasValue)
                    .Select(pv => pv.OWPMatchId.Value.ToString())
                ?? new List<string>()
            );

            await repository.UpdateIsMismatch(OwpMatchIds, date);
        }

        var providerVerificationIds = providerVerification
            .SelectMany(pv => pv.ProviderMatch.Select(pm => pm.ProviderMatchId.ToString()))
            .ToHashSet();

        var unMapInExcel = excelProviderModels
            .Where(e =>
                providerVerification.Any(pv =>
                    pv.ProviderMatch.Any(pm => pm.ProviderMatchId.ToString() == e.ProviderMatchID) &&
                    pv.ProviderMatch.Any(pm => !excelProviderMatchIDs.Contains(pm.ProviderMatchId.ToString()))
                )
                || !providerVerificationIds.Contains(e.ProviderMatchID)
            )
            .Select(e => new ProviderVerificationDifferenceModel
            {
                OWPMatchId = null,
                MatchNo = string.Empty,
                EventDateTime = null,
                KoTime = string.Empty,
                League = string.Empty,
                Home = string.Empty,
                Away = string.Empty,
                LstBookProvider = string.Empty,
                InHouseProviderId = string.Empty,
                Provider = e.Provider,
                ProviderMatchId = e.ProviderMatchID
            })
            .ToList();

        unMapProviderVerification.AddRange(unMapInExcel);

        return unMapProviderVerification;
    }
}
