using Olympus.InfoSiteClient.Models;

namespace OWP.Event.LiveGameAssignment.EventPicking.Builder;

public static class LeagueHomeAwayBuilder
{
    public static string BuildHomeName(CommonMatchEx infositeMatch, SoccerEventSchedule extInfo)
    {
        var isNeutral = extInfo.is_neutral.HasValue && extInfo.is_neutral.Value;
        var isReverse = extInfo.is_team_reverse.HasValue && extInfo.is_team_reverse.Value;
        var homename = isReverse ? infositeMatch.away_name : infositeMatch.home_name;
        return isNeutral ? homename + " (N)" : homename;
    }

    public static string BuildAwayName(CommonMatchEx infositeMatch, SoccerEventSchedule extInfo)
    {
        var isReverse = extInfo.is_team_reverse.HasValue && extInfo.is_team_reverse.Value;
        return isReverse ? infositeMatch.home_name : infositeMatch.away_name;
    }
}
