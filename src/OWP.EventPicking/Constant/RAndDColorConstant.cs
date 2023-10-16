using System.Collections.Immutable;

namespace OWP.EventPicking.Constant;

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
        {DELETE_MATCH,(DELETE_COLOR,WHITE)}
    }.ToImmutableDictionary();

}
