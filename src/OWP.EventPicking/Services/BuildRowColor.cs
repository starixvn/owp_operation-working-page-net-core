using OWP.EventPicking.Models;
using OWP.Extensions;
using System.Text.RegularExpressions;

namespace OWP.EventPicking.Services;

public interface BuildRowColor
{
    void BuildRowColor(List<EventPickingModel> matches);
}

public class BuildRowColorImp : BuildRowColor
{
    public void BuildRowColor(List<EventPickingModel> matches)
        => matches.ForEach(x => { x.RowColor = x.IsBlackList ? Misc.Constant.ColorGray: Misc.Constant.ColorWhite; });

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
               (isContainSaba && sabaMinsPlay.Exists(x => leagueName.Contains(x, StringComparison.OrdinalIgnoreCase)));
    }
}