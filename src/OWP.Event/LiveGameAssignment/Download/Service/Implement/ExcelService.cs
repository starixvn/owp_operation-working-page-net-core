using OWP.Event.LiveGameAssignment.Download.Misc;
using OWP.Event.LiveGameAssignment.Download.Models.EventPicking;
using StringExtension = OWP.Extensions;

namespace OWP.Event.LiveGameAssignment.Download.Service.Implement;

public interface ExcelService
{
    void BuildRowColor(List<MatchExportingNewFormat> matches);

}
public class ExcelServiceImp : ExcelService
{
    public void BuildRowColor(List<MatchExportingNewFormat> matches)
    {
        foreach (var match in matches)
        {
            var provider = StringExtension.StringExtension.RemoveNonAlphaCharacters(match.OtherProvider);
            match.RowColor = BuildRowColor(provider, match.MaxBetHDP, match.IsBlackList);
        }
    }

    /// <summary>
    /// https://nexdev.net/projects/operation-electronic-records/wiki/Auto_Picking_Games
    /// </summary>
    /// <returns></returns>
    public static string BuildRowColor(string provider, string maxbet, bool isBlacklist)
    {

        var color = Constant.ExcelRowColor.First(c => c.HasBlacklist == isBlacklist
             && c.HasProvider == !string.IsNullOrWhiteSpace(provider)
             && c.HasMaxbet == !string.IsNullOrWhiteSpace(maxbet));
        return color.Color;
    }
}