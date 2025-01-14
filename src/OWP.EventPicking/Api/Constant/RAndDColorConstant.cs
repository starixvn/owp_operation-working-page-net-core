using System.Collections.Immutable;

namespace OWP.EventPicking.Api.Constant;

public static class RAndDColorConstant
{
    private const string WHITE = "#FFFFFF";
    private const string GREEN = "#003A13";
    private const string NEW_COLOR = "#0088FF";
    private const string DELETE_COLOR = "#FAC51F";
    private const string UPDATE_COLOR = "#B1D9BE";
    private const string CANCEL_COLOR = "#AB383B";

    public const short NEW_MATCH = 1;
    public const short UPDATE_KO_TIME = 2;
    public const short UPDATE_LEAGUE = 3;
    public const short UPDATE_TEAM = 4;
    public const short UPDATE_TV = 5;
    public const short CANCEL_TV = 6;
    public const short UPDATE_IPTV = 7;
    public const short CANCEL_IPTV = 8;
    public const short UPDATE_OTHER_PROVIDER = 9;
    public const short CANCEL_OTHER_PROVIDER = 10;
    public const short DELETE_MATCH = 11;
    public const short UPDATE_ROUND = 12;
    public const short CANCEL_ROUND = 13;
    public const short UPDATE_ROUND_HAT = 14;
    public const int CANCEL_ROUND_HAT = 15;
    public const short UPDATE_REMARK = 16;
    public const short CANCEL_REMARK = 17;
    public const short UPDATE_HOME_REMARK = 18;
    public const short CANCEL_HOME_REMARK = 19;
    public const short UPDATE_AWAY_REMARK = 20;
    public const short CANCEL_AWAY_REMARK = 21;
    public const short UPDATE_DIVISION = 22;
    public const short CANCEL_DIVISION = 23;
    public const short MOVE_KICKOFF_DATE = 24;
    public const short RENEW_MATCH = 25;
    public const short MATCH_POSTPONED = 26;
    public const short MATCH_CANCELLED = 27;
    public const short UPDATE_ROUND_CONFIRM = 28;
    public const short CANCEL_ROUND_CONFIRM = 29;
    public const short UPDATE_ADMINSITEID = 30;
    public const short CANCEL_ADMINSITEID = 31;
    public const short UPDATE_WEB1 = 32;
    public const short CANCEL_WEB1 = 33;
    public const short UPDATE_WEB2 = 34;
    public const short CANCEL_WEB2 = 35;
    public const short UPDATE_WEB3 = 36;
    public const short CANCEL_WEB3 = 37;
    public const short UPDATE_WEB4 = 38;
    public const short CANCEL_WEB4 = 39;
    public const short UPDATE_LINK = 40;
    public const short CANCEL_LINK = 41;
    public const short UPDATE_CHANNEL= 42;
    public const short CANCEL_CHANNEL = 43;
    public const short UPDATE_AGENT = 44;
    public const short CANCEL_AGENT = 45;
    public const short UPDATE_VENUE = 46;
    public const short CANCEL_VENUE = 47;
    public const short UPDATE_ETPK = 48;
    public const short CANCEL_ETPK = 49;
    public const short UPDATE_NEUTRAL = 50;
    public const short CANCEL_NEUTRAL = 51;

    public static readonly ImmutableDictionary<int, (string, string)> MAP_R_AND_D_COLOR = new Dictionary<int, (string, string)>
    {
        {NEW_MATCH, (NEW_COLOR, WHITE)},
        {UPDATE_KO_TIME, (UPDATE_COLOR, GREEN)},
        {UPDATE_LEAGUE, (UPDATE_COLOR, GREEN)},
        {UPDATE_TEAM, (UPDATE_COLOR, GREEN)},
        {UPDATE_TV, (UPDATE_COLOR, GREEN)},
        {CANCEL_TV, (CANCEL_COLOR, WHITE)},
        {UPDATE_IPTV, (UPDATE_COLOR, GREEN)},
        {CANCEL_IPTV, (CANCEL_COLOR, WHITE)},
        {UPDATE_OTHER_PROVIDER, (UPDATE_COLOR, GREEN)},
        {CANCEL_OTHER_PROVIDER, (CANCEL_COLOR, WHITE)},
        {DELETE_MATCH, (DELETE_COLOR, WHITE)},
        {UPDATE_ROUND, (UPDATE_COLOR, GREEN)},
        {CANCEL_ROUND, (CANCEL_COLOR, WHITE)},
        {UPDATE_ROUND_HAT, (UPDATE_COLOR, GREEN)},
        {CANCEL_ROUND_HAT, (CANCEL_COLOR, WHITE)},
        {UPDATE_REMARK, (UPDATE_COLOR, GREEN)},
        {CANCEL_REMARK, (CANCEL_COLOR, WHITE)},
        {UPDATE_HOME_REMARK, (UPDATE_COLOR, GREEN)},
        {CANCEL_HOME_REMARK, (CANCEL_COLOR, WHITE)},
        {UPDATE_AWAY_REMARK, (UPDATE_COLOR, GREEN)},
        {CANCEL_AWAY_REMARK, (CANCEL_COLOR, WHITE)},
        {UPDATE_DIVISION, (UPDATE_COLOR, GREEN)},
        {CANCEL_DIVISION, (CANCEL_COLOR, WHITE)},
        {MOVE_KICKOFF_DATE, (DELETE_COLOR, WHITE)},
        {RENEW_MATCH, (NEW_COLOR, WHITE)},
        {MATCH_POSTPONED, (CANCEL_COLOR, WHITE)},
        {MATCH_CANCELLED, (CANCEL_COLOR, WHITE)},
        {UPDATE_ROUND_CONFIRM, (UPDATE_COLOR, GREEN)},
        {CANCEL_ROUND_CONFIRM, (CANCEL_COLOR, WHITE)},
        {UPDATE_ADMINSITEID, (UPDATE_COLOR, GREEN)},
        {CANCEL_ADMINSITEID, (CANCEL_COLOR, WHITE)},
        {UPDATE_WEB1, (UPDATE_COLOR, GREEN)},
        {CANCEL_WEB1, (CANCEL_COLOR, WHITE)},
        {UPDATE_WEB2, (UPDATE_COLOR, GREEN)},
        {CANCEL_WEB2, (CANCEL_COLOR, WHITE)},
        {UPDATE_WEB3, (UPDATE_COLOR, GREEN)},
        {CANCEL_WEB3, (CANCEL_COLOR, WHITE)},
        {UPDATE_WEB4, (UPDATE_COLOR, GREEN)},
        {CANCEL_WEB4, (CANCEL_COLOR, WHITE)},
        {UPDATE_LINK, (UPDATE_COLOR, GREEN)},
        {CANCEL_LINK, (CANCEL_COLOR, WHITE)},
        {UPDATE_CHANNEL, (UPDATE_COLOR, GREEN)},
        {CANCEL_CHANNEL, (CANCEL_COLOR, WHITE)},
        {UPDATE_AGENT, (UPDATE_COLOR, GREEN)},
        {CANCEL_AGENT, (CANCEL_COLOR, WHITE)},
        {UPDATE_VENUE, (UPDATE_COLOR, GREEN)},
        {CANCEL_VENUE, (CANCEL_COLOR, WHITE)},
        {UPDATE_ETPK, (UPDATE_COLOR, GREEN)},
        {CANCEL_ETPK, (CANCEL_COLOR, WHITE)},
        {UPDATE_NEUTRAL, (UPDATE_COLOR, GREEN)},
        {CANCEL_NEUTRAL, (CANCEL_COLOR, WHITE)}
    }.ToImmutableDictionary();
}