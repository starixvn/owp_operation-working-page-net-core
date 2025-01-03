using System.Collections.Immutable;

namespace OWP.EventPicking.Api.Misc;

public static class Constant
{
    public const string ColorWhiteHex = "#FFFFFF";
    public const string ColorPinkHex = "#FFCCFF";
    public const string ColorGrayHex = "#808080";
    public const string ColorRedHex = "#F6BEBE";
    public const string ColorYellowHex = "#FFC000";
    public const string ColorBeigeHex = "#e5d9ca";

    public const string ColorWhite = "white";
    public const string ColorGray = "gray";
    public const string ColorBeige = "beige";
    public const string ColorRed = "red";
    public const string ColorYellow = "yellow";

    public const string ContainsCondition = "Contains";

    public static readonly ImmutableDictionary<string, string> ColorPair = new Dictionary<string, string>
    {
        { ColorRed, ColorRedHex },
        { ColorGray, ColorGrayHex },
        { ColorWhite, ColorWhiteHex },
        {ColorYellow,ColorYellowHex},
        {ColorBeige,ColorBeigeHex},
    }.ToImmutableDictionary();

    public static readonly List<string> SpecialOwpName = ["Next Corners", "Next Booking", "(SPECIFIC 15 MINS)"];

    public const int TIME_COL_INDEX = 4;
    public const int HOME_COL_INDEX = 9;
    public const int AWAY_COL_INDEX = 10;
    public const int LEAGUE_COL_INDEX = 11;
    public const int PROVIDER_COL_INDEX = 6;
    public const int OTHER_PROVIDER_COL_INDEX = 7;
    public const int DIVISION_COL_INDEX = 35;
    public const int REMARK_COL_INDEX = 36;
    public const int ROUND_COL_INDEX = 41;
    public const int TV_COL_INDEX = 46;
    public const int IPTV_COL_INDEX = 47;
    public const int HOME_REMARK_COL_INDEX = 38;
    public const int AWAY_REMARK_COL_INDEX = 39;

    public const int START_ROW = 2;

    public static readonly List<int> COL_UPDATE_PINK_COLOR =
    [
        TIME_COL_INDEX, LEAGUE_COL_INDEX, HOME_COL_INDEX, AWAY_COL_INDEX, TV_COL_INDEX, IPTV_COL_INDEX,
        PROVIDER_COL_INDEX
    ];
}