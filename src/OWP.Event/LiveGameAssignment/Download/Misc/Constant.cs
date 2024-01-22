using OWP.Event.LiveGameAssignment.EventPicking.Builder;
using System.Collections.Immutable;

namespace OWP.Event.LiveGameAssignment.Download.Misc;

public static class Constant
{
    public const string EqualsCondition = "Equals";
    public const string ContainsCondition = "Contains";
    public static readonly Dictionary<int, string> StreamNameSrc1InfoSite = new()
    {
        {0, string.Empty },
        {4, "Spanish"},
        {8, "IMG"},
        {9, "Watch & Bet"},
        {12, "SportRadar"},
        {13, "In House"},
        {14, "Player"},
        {15, "Betgenius"},
        {17, "LSports"},
        {18, "Weverdat"},
        {19, "CloundRock"},
        {20, "LiveStreamer"}
    };

    public static readonly ImmutableDictionary<int, string> StreamNameSrc2InfoSite = new Dictionary<int, string>
    {
        {0, string.Empty },
        {11, "Glive"},
    }.ToImmutableDictionary();

    public const string ColorTransparent = "#00FFFFFF";
    private static string colorGray = "#808080";
    private static string colorYellow = "#FFFF99";
    private static string colorRed = "#FF7C80";

    private static List<ColorBuilder> excelRowColor = new()
    {
        new ColorBuilder {HasProvider= true, HasBlacklist= false, HasMaxbet= true, Color= ColorTransparent },
        new ColorBuilder {HasProvider= true, HasBlacklist= false, HasMaxbet= false , Color=ColorTransparent },
        new ColorBuilder {HasProvider= true, HasBlacklist= true, HasMaxbet= true , Color=colorGray },
        new ColorBuilder {HasProvider= true, HasBlacklist= true, HasMaxbet= false , Color=colorGray },
        new ColorBuilder {HasProvider= false, HasBlacklist= false, HasMaxbet= true , Color=colorYellow },
        new ColorBuilder {HasProvider= false, HasBlacklist= false, HasMaxbet= false , Color=colorRed },
        new ColorBuilder {HasProvider= false, HasBlacklist= true, HasMaxbet= true , Color=colorGray },
        new ColorBuilder {HasProvider= false, HasBlacklist= true, HasMaxbet= false , Color=colorGray }
    };

    public const int CornerType = 1;
    public const int BookingType = 2;
    public const int Special15MinsType = 3;

    public static readonly ImmutableDictionary<int, string> SpecialOwpNameDict = new Dictionary<int, string>
    {
        {CornerType,"Next Corners" },
        {BookingType,"Next Booking" },
        {Special15MinsType,"(SPECIFIC 15 MINS)" }
    }.ToImmutableDictionary();

    public static readonly IEnumerable<string> SpecialOwpName = new List<string> { "Next Corners", "Next Booking", "(SPECIFIC 15 MINS)" };

    public static readonly IEnumerable<string> Special15Min = new List<string> { "00:00-15:00", "15:01-30:00", "30:01-45:00", "45:01-60:00", "60:01-75:00", "75:01-90:00" };


    public static List<ColorBuilder> ExcelRowColor => excelRowColor;
}