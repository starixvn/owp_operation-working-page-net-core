using OWP.EventPicking.Misc;
using OWP.EventPicking.Models;
using OWP.Extensions;

namespace OWP.EventPicking.Services;

public interface BuildRowColor
{
    void BuildRowColor(List<EventPickingModel> matches);
}

public class BuildRowColorImp : BuildRowColor
{
    public void BuildRowColor(List<EventPickingModel> matches)
        => matches.ForEach(x => { x.RowColor = BuildRowColor(x); });

    public static string BuildRowColor(EventPickingModel match)
    {
        var provider = StringExtension.RemoveNonAlphaCharacters(match.OtherProvider);
        var conditionMatch = string.IsNullOrWhiteSpace(provider);
        var smallerNoMaxBet = match.MaxBetHDPNumber is < 6000 or null;
        var exceptMatches = match.IsChildMatch || IsSaBaGame(match.LeagueName);

        if (match.IsBlackList)
        {
            return Misc.Constant.ColorGray;
        }
        if (conditionMatch && smallerNoMaxBet && !exceptMatches)
        {
            return Misc.Constant.ColorRed;
        }

        return Misc.Constant.ColorWhite;
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
               (isContainSaba && sabaMinsPlay.Any(x => leagueName.Contains(x, StringComparison.OrdinalIgnoreCase)));
    }
}