using OWP.EventPicking.Api.Constant;
using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api.Services;

public interface BuildRowColor
{
    void BuildRowColor(List<EventPickingModel> matches, bool isEventPicking = false);
}

public class BuildRowColorImp : BuildRowColor
{
    public void BuildRowColor(List<EventPickingModel> matches, bool isEventPicking = false)
    {
        foreach (var x in matches)
        {
            x.RowColor = DetermineRowColor(x, isEventPicking);
        }
    }

    public static bool IsSaBaGame(string leagueName)
    {
        var sabaMinsPlay = new List<string>
        {
            "- 5 Mins Play",
            "- 6 Mins Play",
            "- 15 Mins Play",
            "- 16 Mins Play"
        };
        const string TWENTY_MIN = "- 20 Mins Play";
        var isContainSaba = leagueName.Contains("SABA", StringComparison.OrdinalIgnoreCase);

        return leagueName.Contains(TWENTY_MIN, StringComparison.OrdinalIgnoreCase) ||
               isContainSaba && sabaMinsPlay.Exists(x => leagueName.Contains(x, StringComparison.OrdinalIgnoreCase));
    }
    private string DetermineRowColor(EventPickingModel x, bool isEventPicking)
    {
        if (IsBlackListed(x))
        {
            return Misc.Constant.ColorGray;
        }
            
        if (HasMinorBetTypeAndEventPicking(x, isEventPicking))
        {
            return Misc.Constant.ColorYellow;
        }
            
        if (IsEventStateChanged(x))
        {
            return Misc.Constant.ColorBeige;
        }
            
        return Misc.Constant.ColorWhite;
    }

    private bool IsBlackListed(EventPickingModel x)
    {
        return x.IsAwayBlackList || x.IsHomeBlackList || x.IsLeagueBlackList;
    }

    private bool HasMinorBetTypeAndEventPicking(EventPickingModel x, bool isEventPicking)
    {
        return x.MinorBettypeId.HasValue && isEventPicking;
    }

    private bool IsEventStateChanged(EventPickingModel x)
    {
        return x.IsMovedEventDate || x.IsDeleted || IsMatchStatusChanged(x);
    }

    private static bool IsMatchStatusChanged(EventPickingModel match)
        => match.IsPostponed || IsCancelled(match) || IsSuspended(match);

    private static bool IsCancelled(EventPickingModel match)
        => (!string.IsNullOrEmpty(match.Remark2) && match.Remark2.Contains("Cancel", StringComparison.OrdinalIgnoreCase))
            || match.InfoSiteMatchStatus == InfoSiteMatchStatus.Cancelled;

    private static bool IsSuspended(EventPickingModel match)
        => match.InfoSiteMatchStatus == InfoSiteMatchStatus.Suspended;
}