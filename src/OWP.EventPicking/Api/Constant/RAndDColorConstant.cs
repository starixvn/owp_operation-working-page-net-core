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

    public const int NEW_MATCH = 1;
    public const int UPDATE_KO_TIME = 2;
    public const int UPDATE_LEAGUE = 3;
    public const int UPDATE_TEAM = 4;
    public const int UPDATE_TV = 5;
    public const int CANCEL_TV = 6;
    public const int UPDATE_IPTV = 7;
    public const int CANCEL_IPTV = 8;
    public const int UPDATE_PROVIDER = 9;
    public const int CANCEL_PROVIDER = 10;
    public const int DELETE_MATCH = 11;
    public const int UPDATE_ROUND = 12;
    public const int CANCEL_ROUND = 13;
    public const int UPDATE_ROUND_HAT = 14;
    public const int CANCEL_ROUND_HAT = 15;
    public const int UPDATE_REMARK = 16;
    public const int CANCEL_REMARK = 17;
    public const int UPDATE_HOME_REMARK = 18;
    public const int CANCEL_HOME_REMARK = 19;
    public const int UPDATE_AWAY_REMARK = 20;
    public const int CANCEL_AWAY_REMARK = 21;
    public const int UPDATE_DIVISION = 22;
    public const int CANCEL_DIVISION = 23;
    public const int MOVE_KICKOFF_DATE = 24;
    public const int RENEW_MATCH = 25;

    public static readonly ImmutableDictionary<int, (string, string)> MAP_R_AND_D_COLOR = new Dictionary<int, (string, string)>
    {
        {NEW_MATCH ,(NEW_COLOR,WHITE)},
        {UPDATE_KO_TIME,(UPDATE_COLOR,GREEN)},
        {UPDATE_LEAGUE,(UPDATE_COLOR,GREEN)},
        {UPDATE_TEAM,(UPDATE_COLOR,GREEN)},
        {UPDATE_TV ,(UPDATE_COLOR,GREEN)},
        {CANCEL_TV ,(CANCEL_COLOR,WHITE)},
        {UPDATE_IPTV ,(UPDATE_COLOR,GREEN)},
        {CANCEL_IPTV ,(CANCEL_COLOR,WHITE)},
        {UPDATE_PROVIDER,(UPDATE_COLOR,GREEN)},
        {CANCEL_PROVIDER,(CANCEL_COLOR,WHITE)},
        {DELETE_MATCH,(DELETE_COLOR,WHITE)},
        {UPDATE_ROUND,(UPDATE_COLOR,GREEN)},
        {CANCEL_ROUND,(CANCEL_COLOR,WHITE)},
        {UPDATE_ROUND_HAT,(UPDATE_COLOR,GREEN)},
        {CANCEL_ROUND_HAT,(CANCEL_COLOR,WHITE)},
        {UPDATE_REMARK,(UPDATE_COLOR,GREEN)},
        {CANCEL_REMARK,(CANCEL_COLOR,WHITE)},
        {UPDATE_HOME_REMARK,(UPDATE_COLOR,GREEN)},
        {CANCEL_HOME_REMARK,(CANCEL_COLOR,WHITE)},
        {UPDATE_AWAY_REMARK,(UPDATE_COLOR,GREEN)},
        {CANCEL_AWAY_REMARK,(CANCEL_COLOR,WHITE)},
        {UPDATE_DIVISION,(UPDATE_COLOR,GREEN)},
        {CANCEL_DIVISION,(CANCEL_COLOR,WHITE)},
        {MOVE_KICKOFF_DATE,(DELETE_COLOR,WHITE)},
        {RENEW_MATCH,(NEW_COLOR,WHITE)}
    }.ToImmutableDictionary();
}