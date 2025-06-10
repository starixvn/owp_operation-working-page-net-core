
namespace OWP.ProviderVerification.ProviderVerification.UploadAgent;

public record SubmitAgentRequest(string SessionId, DateTime Date) : Query<ResultT<IList<ProviderVerificationDifferenceModel>>>;

public class SubmitAgentHandler(ICache cache, ProviderVerificationRepository repository) : QueryHandler<SubmitAgentRequest, ResultT<IList<ProviderVerificationDifferenceModel>>>
{
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

        var providerVerification = (await repository.GetProviderVerificationDiff(criteria)).ToList();

        var excelProviderMatchIDs = excelProviderModels.Select(e => e.ProviderMatchID).ToHashSet();

        var unMapProviderVerification = providerVerification
            .Where(pv =>
            {
                var noProviderMatch = pv.ProviderMatch == null || pv.ProviderMatch.Count == 0;

                var notAllMatchesInExcel = !noProviderMatch &&
                                           !pv.ProviderMatch.All(pm => excelProviderMatchIDs.Contains(pm.ProviderMatchId.ToString()));

                var leagueContainsSabaAndCancelStatus = IsSabaLeagueWithCancelStatus(pv);

                var cancelStatusWithEmptyInHouseAndAgentConfirmCancel = IsCancelWithEmptyAdminSiteAndAgentConfirmCancel(pv);

                var isSuspendedOrPostponedMapAgentFile = IsSuspendedOrPostponedMapAgentFile(pv, excelProviderMatchIDs);

                var listAdminSiteProviderName = string.Join(", ", pv.ProviderMatch.Select(p => p.InhouseProviderName));

                var isLSTBookAndAdminSiteDiff = CompareLSTBookAndAdminSiteProvider(listAdminSiteProviderName, pv.AgentConfirmList);

                return noProviderMatch
                       || notAllMatchesInExcel
                       || !leagueContainsSabaAndCancelStatus
                       || !cancelStatusWithEmptyInHouseAndAgentConfirmCancel
                       || isSuspendedOrPostponedMapAgentFile
                       || isLSTBookAndAdminSiteDiff;
            })
            .ToList();

        if (unMapProviderVerification.Count > 0)
        {
            var OwpMatchIds = string.Join(", ",
                unMapProviderVerification
                    .Where(pv => pv.OWPMatchId.HasValue)
                    .Select(pv => pv.OWPMatchId.Value.ToString())
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

    private static bool IsSuspendedOrPostponedMapAgentFile(ProviderVerificationDifferenceModel pv, HashSet<string> excelProviderMatchIDs)
    {
        return (pv.MatchStatus == "Postponed" || pv.MatchStatus == "Suspended") &&
               !string.IsNullOrEmpty(pv.InHouseProviderId) &&
               pv.ProviderMatch.Any(pm => excelProviderMatchIDs.Contains(pm.ProviderMatchId.ToString()));
    }

    private static bool CompareLSTBookAndAdminSiteProvider(string lstBook, string adminSiteProvider)
    {
        var listLSTBook = lstBook.Split(',').Select(x => x.Trim().ToLowerInvariant()).OrderBy(x => x).ToList();
        var listAdminSiteProvider = adminSiteProvider.Split(',').Select(x => x.Trim().ToLowerInvariant()).OrderBy(x => x).ToList();
        return listLSTBook.SequenceEqual(listAdminSiteProvider);
    }

    private static bool IsSabaLeagueWithCancelStatus(ProviderVerificationDifferenceModel pv)
    {
        return !string.IsNullOrEmpty(pv.League) &&
               pv.League.Contains("saba", StringComparison.OrdinalIgnoreCase) &&
               pv.LstBookProvider?.Contains("cancel", StringComparison.OrdinalIgnoreCase) == true;
    }
    private static bool IsCancelWithEmptyAdminSiteAndAgentConfirmCancel(ProviderVerificationDifferenceModel pv)
    {
        return string.Equals(pv.MatchStatus, "cancel", StringComparison.OrdinalIgnoreCase) &&
               pv.AgentConfirmList?.Contains("cancel", StringComparison.OrdinalIgnoreCase) == true;
    }
}
