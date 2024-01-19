using Olympus.InfoSiteClient.Models;
using OWP.Event.LiveGameAssignment.Download.Converter;
using OWP.Event.LiveGameAssignment.Download.Misc;

namespace OWP.Event.LiveGameAssignment.Download.Builder;

public static class CommonBuilder
{
    private static readonly DateTime MinDate = new DateTime(2000, 1, 1);

    public static int? BuildMatchId(CommonMatchEx infositeMatch)
    {
        int? matchId = null;
        if (infositeMatch.ext_info?.create_match_id != 0)
        {
            matchId = infositeMatch.ext_info.create_match_id;
        }

        return matchId;
    }

    public static DateTime BuildGameDate(CommonMatchEx infositeMatch)
    {
        var exInfoGameDate = infositeMatch.ext_info?.game_date != null && infositeMatch.ext_info.game_date.HasValue
            ? infositeMatch.ext_info.game_date.Value
            : MinDate;

        return DateTimeConverter.ConvertTimeToUTC8(exInfoGameDate);
    }

    public static string BuildInfositeIdExport(string homeName, long infositeId)
        => Constant.SpecialOwpName.Any(x => homeName.IsContains(x))
            ? string.Empty
            : infositeId.ToString();
}
