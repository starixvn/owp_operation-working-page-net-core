namespace OWP.EventPicking.Misc;

using System.Collections.Immutable;

public static class Constant
{
    public static string ColorWhiteHex = "#FFFFFF";
    public static string ColorGrayHex = "#808080";
    public static string ColorRedHex = "#F6BEBE";

    public const string ColorWhite = "white";
    public static string ColorGray = "gray";
    public static string ColorRed = "red";

    public const string ContainsCondition = "Contains";

    public static readonly ImmutableDictionary<string, string> ColorDict = new Dictionary<string, string>
    {
        { ColorRed, ColorRedHex },
        { ColorGray, ColorGrayHex },
        { ColorWhite, ColorWhiteHex }
    }.ToImmutableDictionary();

    public static readonly List<string> SpecialOwpName = new() {"Next Corners","Next Booking", "(SPECIFIC 15 MINS)"};
}
