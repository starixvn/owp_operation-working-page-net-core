using System.Collections.Immutable;
using Olympus.OWP.Constants;

namespace OWP.EventPicking.Api.Constant;

public static class RAndDColorConstant
{
    private const string WHITE = "#FFFFFF";
    private const string GREEN = "#003A13";
    private const string NEW_COLOR = "#0088FF";
    private const string DELETE_COLOR = "#FAC51F";
    private const string UPDATE_COLOR = "#B1D9BE";
    private const string CANCEL_COLOR = "#AB383B";

    

    public static readonly ImmutableDictionary<int, (string, string)> MAP_R_AND_D_COLOR = new Dictionary<int, (string, string)>
    {
        {UpdateTypeConstant.NEW_MATCH, (NEW_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_KO_TIME, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.UPDATE_LEAGUE, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.UPDATE_TEAM, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.UPDATE_TV, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_TV, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_IPTV, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_IPTV, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_OTHER_PROVIDER, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_OTHER_PROVIDER, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.DELETE_MATCH, (DELETE_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_ROUND, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_ROUND, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_ROUND_HAT, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_ROUND_HAT, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_REMARK, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_REMARK, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_HOME_REMARK, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_HOME_REMARK, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_AWAY_REMARK, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_AWAY_REMARK, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_DIVISION, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_DIVISION, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.MOVE_KICKOFF_DATE, (DELETE_COLOR, WHITE)},
        {UpdateTypeConstant.RENEW_MATCH, (NEW_COLOR, WHITE)},
        {UpdateTypeConstant.MATCH_POSTPONED, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.MATCH_CANCELLED, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_ROUND_CONFIRM, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_ROUND_CONFIRM, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_ADMINSITEID, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_ADMINSITEID, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_WEB1, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_WEB1, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_WEB2, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_WEB2, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_WEB3, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_WEB3, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_WEB4, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_WEB4, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_LINK, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_LINK, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_CHANNEL, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_CHANNEL, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_AGENT, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_AGENT, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_VENUE, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_VENUE, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_ETPK, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_ETPK, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_NEUTRAL, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_NEUTRAL, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.MATCH_SUSPENDED, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_PROVIDER, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_PROVIDER, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_TOTAL_ROUND, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.CANCEL_TOTAL_ROUND, (CANCEL_COLOR, WHITE)},
        {UpdateTypeConstant.UPDATE_HOME_BAD_RANKING, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.UPDATE_AWAY_BAD_RANKING, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.UPDATE_HOME_RELEGATION, (UPDATE_COLOR, GREEN)},
        {UpdateTypeConstant.UPDATE_AWAY_RELEGATION, (UPDATE_COLOR, GREEN)},
    }.ToImmutableDictionary();
}
