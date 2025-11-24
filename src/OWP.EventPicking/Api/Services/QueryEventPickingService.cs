using Fanex.Logging;
using OWP.Constants;
using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Middleware.Models;

namespace OWP.EventPicking.Api.Services;

public interface QueryEventPickingService
{
    Task<EventPickingResult> Execute(EventPickingRequest request, IList<short> excludedUpdateTypes, CancellationToken cancellationToken = default);

    Task<List<LeagueTurnOver>> GetMatchTurnOver(List<string> leagues, Period period);

}

public class QueryEventPickingServiceImp(
    TGRSService tGrsService,
    BuildMainAndChildMatches buildMainAndChildMatches,
    PermissionService permissionService,
    SyncEventPickingInfo syncEventPickingInfo,
    ILogger logger) : QueryEventPickingService
{
    private const string InfoToHideMatch = "no need include miscmatch";

    public async Task<EventPickingResult> Execute(EventPickingRequest request, IList<short> excludedUpdateTypes, CancellationToken cancellationToken = default)
    {
        var startTime = DateTimeOffset.Now;
        var matches = await buildMainAndChildMatches.GetMainEvent(request.Date);
        var endTime = DateTimeOffset.Now;
        await logger.InfoAsync($"Query Event Picking New - exec time: {(endTime - startTime).TotalSeconds}s");
        var eventPickingPolicy = permissionService.GetPermissions(FunctionKeys.EventPickingNew);
        var turnoverMarginPolicy = eventPickingPolicy.GetPermissionModel(FunctionKeys.EventPickingNew_EventAndPerformance);
        var validMatch = FilterAllMatchesByRequest(matches, request);

        var generatedMatches = await buildMainAndChildMatches.GetChildEventEventPicking(request.Date);
        var allMatches = SpecialProductHelper.CombineMainAndChildMatchAndAddMatchNo(validMatch, generatedMatches);

        if (turnoverMarginPolicy.IsRead && request.IsLoadTurnOver)
        {
            await CalculateTurnOver_Margin(request, allMatches);
        }

        HideNoNeedIncludeMiscMatch(ref allMatches, InfoToHideMatch);

        var matchesByDate = SelectMatchOfferInfos(ExcludeUpdateType(matches, excludedUpdateTypes));
        var filteredMatches = ExcludeUpdateType(allMatches, excludedUpdateTypes);

        filteredMatches = await SyncInfoFromLeagueSetting(filteredMatches, request, cancellationToken);

        return new EventPickingResult
        {
            MatchesByDate = matchesByDate,
            FilteredMatches = filteredMatches
        };
    }

    public async Task<List<LeagueTurnOver>> GetMatchTurnOver(List<string> leagues, Period period)
    {
        var eventPickingPolicy = permissionService.GetPermissions(FunctionKeys.EventPickingNew);
        var turnoverMarginPolicy = eventPickingPolicy.GetPermissionModel(FunctionKeys.EventPickingNew_EventAndPerformance);

        if (turnoverMarginPolicy.IsRead)
        {
            var twOneMonth = GetTurnover_WinLoss_TGRS(period.OneMonth, leagues);
            var twThreeMonths = GetTurnover_WinLoss_TGRS(period.ThreeMonths, leagues);
            var twSixMonths = GetTurnover_WinLoss_TGRS(period.SixMonths, leagues);
            var twTwelveMonths = GetTurnover_WinLoss_TGRS(period.TwelveMonths, leagues);
            await Task.WhenAll(twOneMonth, twThreeMonths, twSixMonths, twTwelveMonths);

            var leagueTurnOvers = new List<LeagueTurnOver>();

            foreach (var leagueName in leagues)
            {
                var leagueTurnOver = InitLeagueTurnOver(period, leagueName);
                leagueTurnOvers.Add(leagueTurnOver);

                var oneMonthData = twOneMonth.Result?.FirstOrDefault(x => x.LeagueName == leagueName);
                PopulatePeriodData(oneMonthData, leagueTurnOver, "1m");

                var threeMonthsData = twThreeMonths.Result?.FirstOrDefault(x => x.LeagueName == leagueName);
                PopulatePeriodData(threeMonthsData, leagueTurnOver, "3m");

                var sixMonthsData = twSixMonths.Result?.FirstOrDefault(x => x.LeagueName == leagueName);
                PopulatePeriodData(sixMonthsData, leagueTurnOver, "6m");
                // Get data for 12 months period
                var twelveMonthsData = twTwelveMonths.Result?.FirstOrDefault(x => x.LeagueName == leagueName);
                PopulatePeriodData(twelveMonthsData, leagueTurnOver, "12m");

            }

            return leagueTurnOvers;
        }
        return new List<LeagueTurnOver>();
    }

    public async Task CalculateTurnOver_Margin(EventPickingRequest request, List<EventPickingModel> matches)
    {
        var twOneMonth = GetTurnover_WinLoss_TGRS(request.Period.OneMonth, matches);
        var twThreeMonths = GetTurnover_WinLoss_TGRS(request.Period.ThreeMonths, matches);
        var twSixMonths = GetTurnover_WinLoss_TGRS(request.Period.SixMonths, matches);
        var twTwelveMonths = GetTurnover_WinLoss_TGRS(request.Period.TwelveMonths, matches);
        await Task.WhenAll(twOneMonth, twThreeMonths, twSixMonths, twTwelveMonths);
        foreach (var match in matches)
        {
            InitTurnover_MarginInfo(request.Period, match);
            CalculateTurnover_WinLoss_PeriodOneMonth(twOneMonth.Result, match);
            CalculateTurnover_WinLoss_PeriodThreeMonth(twThreeMonths.Result, match);
            CalculateTurnover_WinLoss_PeriodSixMonth(twSixMonths.Result, match);
            CalculateTurnover_WinLoss_PeriodTwelveMonths(twTwelveMonths.Result, match);
        }
    }

    public async Task CalculateTurnOver_Margin(List<string> leagues, Period period)
    {
        var twOneMonth = GetTurnover_WinLoss_TGRS(period.OneMonth, leagues);
        var twThreeMonths = GetTurnover_WinLoss_TGRS(period.ThreeMonths, leagues);
        var twSixMonths = GetTurnover_WinLoss_TGRS(period.SixMonths, leagues);
        var twTwelveMonths = GetTurnover_WinLoss_TGRS(period.TwelveMonths, leagues);
        await Task.WhenAll(twOneMonth, twThreeMonths, twSixMonths, twTwelveMonths);
    }

    public static void CalculateTurnover_WinLoss_PeriodOneMonth(IList<TurnOverWinLossModel> tw, EventPickingModel match)
    {
        var turnover_winLoss = tw.FirstOrDefault(t => string.Equals(t.LeagueName, match.LeagueName));
        Utils.CalculateValueTurnover(turnover_winLoss, match.TurnoverOneMonth);
        Utils.CalculateValueWinLoss(turnover_winLoss, match.MarginOneMonth);
    }

    public static void CalculateTurnover_WinLoss_PeriodThreeMonth(IList<TurnOverWinLossModel> tw, EventPickingModel match)
    {
        var turnover_winLoss = tw.FirstOrDefault(t => string.Equals(t.LeagueName, match.LeagueName));
        Utils.CalculateValueTurnover(turnover_winLoss, match.TurnoverThreeMonths);
        Utils.CalculateValueWinLoss(turnover_winLoss, match.MarginThreeMonths);
    }

    public static void CalculateTurnover_WinLoss_PeriodSixMonth(IList<TurnOverWinLossModel> tw, EventPickingModel match)
    {
        var turnover_winLoss = tw.FirstOrDefault(t => string.Equals(t.LeagueName, match.LeagueName));
        Utils.CalculateValueTurnover(turnover_winLoss, match.TurnoverSixMonths);
        Utils.CalculateValueWinLoss(turnover_winLoss, match.MarginSixMonths);
    }

    public static void CalculateTurnover_WinLoss_PeriodTwelveMonths(IList<TurnOverWinLossModel> tw, EventPickingModel match)
    {
        var turnover_winLoss = tw.FirstOrDefault(t => string.Equals(t.LeagueName, match.LeagueName));
        Utils.CalculateValueTurnover(turnover_winLoss, match.TurnoverTwelveMonths);
        Utils.CalculateValueWinLoss(turnover_winLoss, match.MarginTwelveMonths);
    }

    public async Task<IList<TurnOverWinLossModel>> GetTurnover_WinLoss_TGRS(TimeRange period, List<EventPickingModel> matches)
    {
        if (period is not null)
        {
            var result = await tGrsService.GetTurnOverWinLoss(new GetTurnOverWinLossCriteria
            {
                SportType = "Soccer",
                FromDate = period.FromDate,
                ToDate = period.ToDate,
                LeagueName = GetLeagueName(matches)
            });
            matches.ForEach(x => x.IsTGRUnAvailable = result.Item2);
            return result.Item1;
        }
        return new List<TurnOverWinLossModel>();
    }

    public async Task<IList<TurnOverWinLossModel>> GetTurnover_WinLoss_TGRS(TimeRange period, List<string> leagues)
    {
        if (period is not null)
        {
            var result = await tGrsService.GetTurnOverWinLoss(new GetTurnOverWinLossCriteria
            {
                SportType = "Soccer",
                FromDate = period.FromDate,
                ToDate = period.ToDate,
                LeagueName = leagues
            });
            return result.Item1;
        }
        return new List<TurnOverWinLossModel>();
    }


    private static void InitTurnover_MarginInfo(Period period, EventPickingModel match)
    {
        if (period.OneMonth is not null)
        {
            match.TurnoverOneMonth = new TurnOverInfo("1m");
            match.MarginOneMonth = new MarginInfo("1m");
        }
        if (period.ThreeMonths is not null)
        {
            match.TurnoverThreeMonths = new TurnOverInfo("3m");
            match.MarginThreeMonths = new MarginInfo("3m");
        }
        if (period.SixMonths is not null)
        {
            match.TurnoverSixMonths = new TurnOverInfo("6m");
            match.MarginSixMonths = new MarginInfo("6m");
        }
        if (period.TwelveMonths is not null)
        {
            match.TurnoverTwelveMonths = new TurnOverInfo("12m");
            match.MarginTwelveMonths = new MarginInfo("12m");
        }
    }

    private static LeagueTurnOver InitLeagueTurnOver(Period period, string leagueName)
    {
        var leagueTurnOver = new LeagueTurnOver
        {
            LeagueName = leagueName,
            TurnoverPeriod = new List<TurnOverInfo>(),
            MarginPeriod = new List<MarginInfo>()
        };

        if (period.OneMonth is not null)
        {
            leagueTurnOver.TurnoverPeriod.Add(new TurnOverInfo("1m"));
            leagueTurnOver.MarginPeriod.Add(new MarginInfo("1m"));
        }
        if (period.ThreeMonths is not null)
        {
            leagueTurnOver.TurnoverPeriod.Add(new TurnOverInfo("3m"));
            leagueTurnOver.MarginPeriod.Add(new MarginInfo("3m"));
        }
        if (period.SixMonths is not null)
        {
            leagueTurnOver.TurnoverPeriod.Add(new TurnOverInfo("6m"));
            leagueTurnOver.MarginPeriod.Add(new MarginInfo("6m"));
        }
        if (period.TwelveMonths is not null)
        {
            leagueTurnOver.TurnoverPeriod.Add(new TurnOverInfo("12m"));
            leagueTurnOver.MarginPeriod.Add(new MarginInfo("12m"));
        }

        return leagueTurnOver;
    }

    private static void PopulatePeriodData(TurnOverWinLossModel periodData, LeagueTurnOver leagueTurnOver, string periodTitle)
    {
        if (periodData != null)
        {
            var turnoverInfo = leagueTurnOver.TurnoverPeriod.First(x => x.Title == periodTitle);
            var marginInfo = leagueTurnOver.MarginPeriod.First(x => x.Title == periodTitle);
            Utils.CalculateValueTurnover(periodData, turnoverInfo);
            Utils.CalculateValueWinLoss(periodData, marginInfo);
        }
    }



    private static IList<string> GetLeagueName(List<EventPickingModel> matches)
    => matches.Select(x => x.LeagueName).Distinct().ToList();

    private async Task<List<EventPickingModel>> SyncInfoFromLeagueSetting(
        List<EventPickingModel> filteredMatches,
        EventPickingRequest request,
        CancellationToken cancellationToken)
    {
        await syncEventPickingInfo.Execute(filteredMatches, cancellationToken);

        return filteredMatches
            .Where(x => CheckCondition1(x, request))
            .ToList();
    }

    private static List<EventPickingModel> FilterAllMatchesByRequest(List<EventPickingModel> matches, EventPickingRequest request)
    => matches
        .Where(x => ValidMatch(x, request))
        .ToList();

    public static bool ValidMatch(EventPickingModel match, EventPickingRequest condition)
        => CheckCondition2(match, condition) && CheckCondition3(match, condition);

    private static bool CheckCondition1(EventPickingModel match, EventPickingRequest condition)
        => condition.SelectGame.Contains(ConvertBoolToInt(match.IsNoOffer));

    private static bool CheckCondition2(EventPickingModel match, EventPickingRequest condition)
        => condition.FilterMatch.Contains(match.RowColor, StringComparer.OrdinalIgnoreCase)
           && condition.HasProvider.Contains(match.HasProvider, StringComparer.OrdinalIgnoreCase)
           && condition.HasOtherProvider.Contains(match.HasOtherProvider, StringComparer.OrdinalIgnoreCase)
           && condition.FilterMatchCategory.Contains(ConvertBoolToInt(match.IsSaba));

    private static bool CheckCondition3(EventPickingModel match, EventPickingRequest condition)
        => CheckUpdateType(match, condition);

    private static bool CheckUpdateType(EventPickingModel match, EventPickingRequest condition)
    {
        if (condition.UpdateType.Exists(string.IsNullOrEmpty) &&
            (match.RAndDInfo.Count == 0 || match.RAndDInfo.Exists(info => string.IsNullOrEmpty(info.Group))))
        {
            return true;
        }

        return match.RAndDInfo
            .Where(info => !string.IsNullOrEmpty(info.Group))
            .Any(info => condition.UpdateType.Contains(info.Group) &&
                         IsMatchingUpdateType(info.UpdateTypeId, match.InfoSiteMatchStatus));
    }

    private static bool IsMatchingUpdateType(int updateTypeId, byte? matchStatus)
    {
        return updateTypeId switch
        {
            UpdateTypeConstant.MATCH_POSTPONED => matchStatus == InfoSiteMatchStatus.Postponed,
            UpdateTypeConstant.MATCH_CANCELLED => matchStatus == InfoSiteMatchStatus.Cancelled,
            UpdateTypeConstant.MATCH_SUSPENDED => matchStatus == InfoSiteMatchStatus.Suspended,
            _ => IsOtherMatch(updateTypeId)
        };
    }

    private static bool IsOtherMatch(int updateTypeId)
    {
        return updateTypeId != UpdateTypeConstant.MATCH_POSTPONED &&
               updateTypeId != UpdateTypeConstant.MATCH_CANCELLED &&
               updateTypeId != UpdateTypeConstant.MATCH_SUSPENDED;
    }

    private static List<EventPickingModel> ExcludeUpdateType(List<EventPickingModel> eventPicking, IList<short> excludedUpdateTypes)
    {
        foreach (var @event in eventPicking)
        {
            var filteredRAndDInfo = @event.RAndDInfo?
                .Where(e => !excludedUpdateTypes.Contains((short)e.UpdateTypeId))
                .ToList();

            if (@event.InfoSiteMatchStatus == InfoSiteMatchStatus.Normal)
            {
                filteredRAndDInfo = filteredRAndDInfo
                    .Where(r => r.UpdateTypeId != UpdateTypeConstant.MATCH_POSTPONED &&
                                r.UpdateTypeId != UpdateTypeConstant.MATCH_CANCELLED &&
                                r.UpdateTypeId != UpdateTypeConstant.MATCH_SUSPENDED)
                    .ToList();
            }

            @event.RAndDInfo = filteredRAndDInfo;
        }
        return eventPicking;
    }

    private static int ConvertBoolToInt(bool? value) => value == true ? 1 : 0;

    private static List<MatchOfferInfo> SelectMatchOfferInfos(List<EventPickingModel> matches)
        => matches.Select(e => new MatchOfferInfo(e.InfoSiteMatchId, e.IsNoOffer ?? false, e.EventDateTime)).ToList();

    private static void HideNoNeedIncludeMiscMatch(ref List<EventPickingModel> allMatches, string infoToHideMatch)
    {
        allMatches = allMatches.Where(item => string.IsNullOrEmpty(item.Remark2) || !item.Remark2.ToLowerInvariant().Contains(infoToHideMatch)).ToList();
    }
}