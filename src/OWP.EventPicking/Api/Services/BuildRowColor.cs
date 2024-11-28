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
            if (x.IsAwayBlackList || x.IsHomeBlackList || x.IsLeagueBlackList)
            {
                x.RowColor = Misc.Constant.ColorGray;
            }
            else if (x.MinorBettypeId.HasValue && isEventPicking)
            {
                x.RowColor = Misc.Constant.ColorYellow;
            }
            else if (x.IsMovedEventDate || IsPostponed(x) || IsCancelled(x))
            {
                x.RowColor = Misc.Constant.ColorBeige;
            }
            else
            {
                x.RowColor = Misc.Constant.ColorWhite;
            }
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

    private static bool IsPostponed(EventPickingModel match)
        => (!string.IsNullOrEmpty(match.Remark2) && match.Remark2.Contains("Postponed"))
            || match.InfoSiteMatchStatus == InfoSiteMatchStatus.Postponed;

    private static bool IsCancelled(EventPickingModel match)
        => (!string.IsNullOrEmpty(match.Remark2) && match.Remark2.Contains("Cancel", StringComparison.OrdinalIgnoreCase))
            || match.InfoSiteMatchStatus == InfoSiteMatchStatus.Cancelled;
}