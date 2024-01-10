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
        var turnoverMarginPolicy = eventPickingPolicy.GetPermissionModel(FunctionKeys.EventPickingNew_ShowSensitiveColumn);
        var validMatch = FilterAllMatchesByRequest(matches, request);
        if (turnoverMarginPolicy.IsRead)
        {

            var turnOver_WinLoss = await tGRSService.GetTurnOverWinLoss(new GetTurnOverWinLossCriteria()
            {
                //FromDate = request.FromDate,
                //ToDate = request.ToDate,
                LeagueName = GetLeagueName(validMatch)
            });
            CalculateTurnOver_Margin(validMatch, turnOver_WinLoss);
        }
        return validMatch;
    }

    private static void CalculateTurnOver_Margin(IList<EventPickingModel> matches, IList<TurnOverWinLossModel> turnOverWinLossModels)
    {
        // TODO
        foreach (var match in matches)
        {
            var turnOver_winLoss = turnOverWinLossModels.FirstOrDefault(t => string.Equals(t.LeagueName, match.LeagueName));
            if (turnOver_winLoss is not null)
            {
                match.TurnOver = turnOver_winLoss.TurnOver;
                match.Margin = 0;
            }
        }
    }

    private static IList<string> GetLeagueName(List<EventPickingModel> matches)
    => matches.Select(x => x.LeagueName).Distinct().ToList();

    private List<EventPickingModel> FilterAllMatchesByRequest(List<EventPickingModel> matches, EventPickingRequest request)
    => matches
        .Where(x => ValidMatch(x, request))
        .ToList();

    private bool ValidMatch(EventPickingModel match, EventPickingRequest condition)
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