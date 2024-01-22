using Olympus.InfoSiteClient.Models;
using OWP.Event.LiveGameAssignment.Download.Converter;
using OWP.Event.LiveGameAssignment.Download.Misc;

namespace OWP.Event.LiveGameAssignment.EventPicking.Service;

public static class CommonBuilder
{
    private static readonly DateTimeOffset MinDate = new(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

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

        return DateTimeConverter.ConvertTimeToUTC8(exInfoGameDate.DateTime);
    }

    public static string BuildInfositeIdExport(string homeName, long infositeId)
        => Constant.SpecialOwpName.ToList().Exists(x => homeName.IsContains(x))
            ? string.Empty
            : infositeId.ToString();
}
