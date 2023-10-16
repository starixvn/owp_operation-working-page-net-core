using System.Collections.Immutable;

namespace OWP.EventPicking.Misc;

public static class Constant
{
    public const string ColorWhiteHex = "#FFFFFF";
    public const string ColorPinkHex = "#FFCCFF";
    public const string ColorGrayHex = "#808080";
    public const string ColorRedHex = "#F6BEBE";

    public const string ColorWhite = "white";
    public const string ColorGray = "gray";
    public const string ColorRed = "red";

    public const string ContainsCondition = "Contains";

    public static readonly ImmutableDictionary<string, string> ColorPair = new Dictionary<string, string>
    {
        { ColorRed, ColorRedHex },
        { ColorGray, ColorGrayHex },
        { ColorWhite, ColorWhiteHex }
    }.ToImmutableDictionary();

    public static readonly List<string> SpecialOwpName = new() { "Next Corners", "Next Booking", "(SPECIFIC 15 MINS)" };

    public const int TIME_COL_INDEX = 4;
    public const int HOME_COL_INDEX = 6;
    public const int AWAY_COL_INDEX = 7;
    public const int LEAGUE_COL_INDEX = 8;
    public const int TV_COL_INDEX = 38;
    public const int PROVIDER_COL_INDEX = 32;
    public const int IPTV_COL_INDEX = 40;

    public const int REMARK_COL_INDEX = 35;
    public const int START_ROW = 2;

    public static readonly List<int> COL_UPDATE_PINK_COLOR = new() { TIME_COL_INDEX,LEAGUE_COL_INDEX,HOME_COL_INDEX,AWAY_COL_INDEX,TV_COL_INDEX,IPTV_COL_INDEX,PROVIDER_COL_INDEX
    };
}
