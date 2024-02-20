using OWP.Constants;
using OWP.EventPicking.Criteria;
using OWP.EventPicking.Middleware.Models;
using OWP.EventPicking.Models;
using OWP.Extensions;
using OWP.Services;

namespace OWP.EventPicking.Services;

public interface QueryEventPickingService
{
    Task<List<EventPickingModel>> Execute(EventPickingRequest request);
}

public class QueryEventPickingServiceImp : QueryEventPickingService
{
    private readonly BuildMainAndChildMatches buildMainAndChildMatches;

    private readonly PermissionService permissionService;

    private readonly TGRSService tGRSService;

    public QueryEventPickingServiceImp(TGRSService tGRSService, BuildMainAndChildMatches buildMainAndChildMatches, PermissionService permissionService)
    {
        this.buildMainAndChildMatches = buildMainAndChildMatches;
        this.permissionService = permissionService;
        this.tGRSService = tGRSService;
    }

    public async Task<List<EventPickingModel>> Execute(EventPickingRequest request)
    {
        var matches = await buildMainAndChildMatches.GetMainEvent(request.Date);
        var eventPickingPolicy = permissionService.GetPermissions(FunctionKeys.EventPickingNew);
        var turnoverMarginPolicy = eventPickingPolicy.GetPermissionModel(FunctionKeys.EventPickingNew_EventAndPerformance);
        var validMatch = FilterAllMatchesByRequest(matches, request);
        if (turnoverMarginPolicy.IsRead)
        {
            await CalculateTurnOver_Margin(request, validMatch);
        }
        return validMatch;
    }

    private async Task CalculateTurnOver_Margin(EventPickingRequest request, List<EventPickingModel> matches)
    {
        var twOneMonth = await GetTurnover_WinLoss_TGRS(request.PeriodOneMonth, matches);
        var twThreeMonths = await GetTurnover_WinLoss_TGRS(request.PeriodThreeMonths, matches);
        var twSixMonths = await GetTurnover_WinLoss_TGRS(request.PeriodSixMonths, matches);
        var twTwelveMonths = await GetTurnover_WinLoss_TGRS(request.PeriodTwelveMonths, matches);

        foreach (var match in matches)
        {
            CalculateTurnover_WinLoss_PeriodOneMonth(twOneMonth, match);
            CalculateTurnover_WinLoss_PeriodThreeMonth(twThreeMonths, match);
            CalculateTurnover_WinLoss_PeriodSixMonth(twSixMonths, match);
            CalculateTurnover_WinLoss_PeriodTwelveMonths(twTwelveMonths, match);
        }
    }

    public static void CalculateTurnover_WinLoss_PeriodOneMonth(IList<TurnOverWinLossModel> tw, EventPickingModel match)
    {
        var turnover_winLoss = tw.FirstOrDefault(t => string.Equals(t.LeagueName, match.LeagueName));

        Utils.CalculateValueTurnover_WinLoss(
            turnover_winLoss,
            match.TurnoverOneMonth,
            match.MarginOneMonth);

        Utils.CalculateColorTurnover_WinLoss(turnover_winLoss, match.TurnoverOneMonth,
            match.MarginOneMonth);
    }
    public static void CalculateTurnover_WinLoss_PeriodThreeMonth(IList<TurnOverWinLossModel> tw, EventPickingModel match)
    {
        var turnover_winLoss = tw.FirstOrDefault(t => string.Equals(t.LeagueName, match.LeagueName));
        Utils.CalculateValueTurnover_WinLoss(
            turnover_winLoss,
            match.TurnoverThreeMonths,
            match.MarginThreeMonths);
        Utils.CalculateColorTurnover_WinLoss(turnover_winLoss, match.TurnoverThreeMonths,
            match.MarginThreeMonths);
    }
    public static void CalculateTurnover_WinLoss_PeriodSixMonth(IList<TurnOverWinLossModel> tw, EventPickingModel match)
    {
        var turnover_winLoss = tw.FirstOrDefault(t => string.Equals(t.LeagueName, match.LeagueName));
        Utils.CalculateValueTurnover_WinLoss(
            turnover_winLoss,
            match.TurnoverSixMonths,
            match.MarginSixMonths);
        Utils.CalculateColorTurnover_WinLoss(
            turnover_winLoss,
            match.TurnoverSixMonths,
            match.MarginSixMonths);
    }
    public static void CalculateTurnover_WinLoss_PeriodTwelveMonths(IList<TurnOverWinLossModel> tw, EventPickingModel match)
    {
        var turnover_winLoss = tw.FirstOrDefault(t => string.Equals(t.LeagueName, match.LeagueName));
        Utils.CalculateValueTurnover_WinLoss(
            turnover_winLoss,
            match.TurnoverTwelveMonths,
            match.MarginTwelveMonths);
        Utils.CalculateColorTurnover_WinLoss(
            turnover_winLoss,
            match.TurnoverTwelveMonths,
            match.MarginTwelveMonths);
    }

    public async Task<IList<TurnOverWinLossModel>> GetTurnover_WinLoss_TGRS(Period period, List<EventPickingModel> matches)
    {
        if (period is not null)
        {
            return await tGRSService.GetTurnOverWinLoss(new()
            {
                SportType = "Soccer",
                FromDate = period.FromDate,
                ToDate = period.ToDate,
                LeagueName = GetLeagueName(matches)
            });
        }
        return new List<TurnOverWinLossModel>();
    }

    private static IList<string> GetLeagueName(List<EventPickingModel> matches)
    => matches.Select(x => x.LeagueName).Distinct().ToList();

    private static List<EventPickingModel> FilterAllMatchesByRequest(List<EventPickingModel> matches, EventPickingRequest request)
    => matches
        .Where(x => ValidMatch(x, request))
        .ToList();

    private static bool ValidMatch(EventPickingModel match, EventPickingRequest condition)
        => CheckCondition1(match, condition) && CheckCondition2(match, condition) && CheckCondition3(match, condition);

    private static bool CheckCondition1(EventPickingModel match, EventPickingRequest condition)
        => condition.SelectGame.Contains(ConvertBoolToInt(match.IsNoOffer))
           && condition.FilterMatchCategory.Contains(ConvertBoolToInt(match.IsSaba));

    private static bool CheckCondition2(EventPickingModel match, EventPickingRequest condition)
        => condition.FilterMatch.Contains(match.RowColor, StringComparer.OrdinalIgnoreCase)
           && condition.HasProvider.Contains(match.HasProvider, StringComparer.OrdinalIgnoreCase)
           && condition.HasOtherProvider.Contains(match.HasOtherProvider, StringComparer.OrdinalIgnoreCase);

    private static bool CheckCondition3(EventPickingModel match, EventPickingRequest condition)
        => condition.League.Contains(match.OWPLeagueId) && CheckUpdateType(match, condition);

    private static bool CheckUpdateType(EventPickingModel match, EventPickingRequest condition)
    {
        if (condition.UpdateType.Contains(null) && match.RAndDInfos.Count == 0)
        {
            return true;
        }

        return match.RAndDInfos
        .Select(x => x.UpdateTypeId)
        .Any(x => condition.UpdateType.Contains(x));
    }

    private static int ConvertBoolToInt(bool value) => value ? 1 : 0;
}