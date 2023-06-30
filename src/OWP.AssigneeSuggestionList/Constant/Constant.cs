using OWP.AssigneeSuggestionList.Models;
using System.Collections.Immutable;

namespace OWP.AssigneeSuggestionList.Constant;

public static class ColorCodeConst
{
    public const string MJ = "#99CCFF";
    public const string ONE_GAME = "#92D050";
    public const string TWO_GAME = "#FF6699";
    public const string THREE_GAME = "#F79646";
    public const string ERROR = "#FFFFFF";
    public const string FIRST_HALF = "#B1A0C7";
    public const string FANTASY = "#E6B8B7";
    public const string CORNER = "#C00000";
    public const string WINNER = "#036541";
    public const string X_MINS = "#0070C0";
    public const string ET_PK = "#000000";
    public const string SHIFT = "#BFBFBF";

    public const string TEXT_COLOR_BLACK = "#444444";
    public const string TEXT_COLOR_WHITE = "#FFFFFF";

    public const string TEXT_MJ = TEXT_COLOR_BLACK;
    public const string TEXT_ONE_GAME = TEXT_COLOR_BLACK;
    public const string TEXT_TWO_GAME = TEXT_COLOR_BLACK;
    public const string TEXT_THREE_GAME = TEXT_COLOR_BLACK;
    public const string TEXT_ERROR = "#FF0000";
    public const string TEXT_FIRST_HALF = TEXT_COLOR_BLACK;
    public const string TEXT_FANTASY = TEXT_COLOR_BLACK;
    public const string TEXT_CORNER = TEXT_COLOR_WHITE;
    public const string TEXT_WINNER = TEXT_COLOR_WHITE;
    public const string TEXT_X_MINS = TEXT_COLOR_WHITE;
    public const string TEXT_ET_PK = "#FFEA29";

    // Saba
    private const string SABA_5MINS = "#7030A0";
    private const string SABA_FUTSAL = "#00FFCC";
    private const string SABA_M88 = "#FCD5B4";
    private const string SABA_1G = "#FFFFCC";
    private const string SABA_2G = "#A34262";
    private const string SABA_3G = "#FFC000";
    private const string SABA_4G = "#33CCCC";
    private const string SABA_16MINS = "#00B050";

    public static IList<ColorExplainModel> ColorExplain => new List<ColorExplainModel>
    {
         new ("MJ", MJ, TEXT_COLOR_BLACK),
         new ("1G", ONE_GAME, TEXT_COLOR_BLACK),
         new ("2G", TWO_GAME, TEXT_COLOR_BLACK),
         new ("3G", THREE_GAME, TEXT_COLOR_BLACK),
         new ("1st half", FIRST_HALF, TEXT_COLOR_BLACK),
         new ("Fantasy", FANTASY, TEXT_COLOR_BLACK),
         new ("CR/NCR", CORNER, TEXT_COLOR_WHITE),
         new ("15M OU", X_MINS, TEXT_COLOR_WHITE),
         new ("5M", SABA_5MINS, TEXT_COLOR_WHITE),
         new ("Saba Futsal", SABA_FUTSAL, TEXT_COLOR_BLACK),
         new ("M88", SABA_M88, TEXT_COLOR_BLACK),
         new ("16M", SABA_16MINS, TEXT_COLOR_WHITE),
         new ("*1G", SABA_1G, TEXT_COLOR_BLACK),
         new ("*2G", SABA_2G, TEXT_COLOR_WHITE),
         new ("*3G", SABA_3G, TEXT_COLOR_BLACK),
         new ("*4G", SABA_4G, TEXT_COLOR_BLACK)
    };


    public static readonly (string, string) ErrorColor = (TEXT_COLOR_WHITE, TEXT_ERROR);
    public static readonly ImmutableDictionary<MatchType, (string, string)> ColorByGroupName = new Dictionary<MatchType, (string, string)>
    {
        { MatchType.Saba5Mins, (SABA_5MINS, TEXT_COLOR_WHITE) },
        { MatchType.SabaFutsal6Mins, (SABA_FUTSAL, TEXT_COLOR_BLACK) },
        { MatchType.SabaM8816Mins, (SABA_M88, TEXT_COLOR_BLACK) },
        { MatchType.SabaLink1G, (SABA_1G, TEXT_COLOR_BLACK) },
        { MatchType.SabaLink2G, (SABA_2G, TEXT_COLOR_WHITE) },
        { MatchType.SabaLink3G, (SABA_3G, TEXT_COLOR_BLACK) },
        { MatchType.SabaLink4G, (SABA_4G, TEXT_COLOR_BLACK) },
        { MatchType.Saba16Mins, (SABA_16MINS, TEXT_COLOR_WHITE) }
    }.ToImmutableDictionary();
}

public static class ETPKConstant
{
    public const string ETPK = "ET/PK";

    public const string ET = "ET";

    public const string PK = "PK";

    public const string ETPK_Confirm = "Pls cfm this game have ET, PK or not";
}

public static class WorkingShiftConst
{
    public static readonly ImmutableDictionary<int, string> PropertyDict = new Dictionary<int, string>
    {
        {0,"Yesterday_02h"},
        {1,"Yesterday_03h"},
        {2,"Yesterday_04h"},
        {3,"Yesterday_05h"},
        {4,"Yesterday_06h"},
        {5,"Yesterday_07h"},
        {6,"Yesterday_08h"},
        {7,"Yesterday_09h"},
        {8,"Yesterday_10h"},
        {9,"Yesterday_11h"},
        {10,"Today_12h"},
        {11,"Today_13h"},
        {12,"Today_14h"},
        {13,"Today_15h"},
        {14,"Today_16h"},
        {15,"Today_17h"},
        {16,"Today_18h"},
        {17,"Today_19h"},
        {18,"Today_20h"},
        {19,"Today_21h"},
        {20,"Today_22h"},
        {21,"Today_23h"},
        {22,"Today_00h"},
        {23,"Today_01h"},
        {24,"Today_02h"},
        {25,"Today_03h"},
        {26,"Today_04h"},
        {27,"Today_05h"},
        {28,"Today_06h"},
        {29,"Today_07h"},
        {30,"Today_08h"},
        {31,"Today_09h"},
        {32,"Today_10h"},
        {33,"Today_11h"},
        {34,"Tomorrow_12h"},
        {35,"Tomorrow_13h"},
        {36,"Tomorrow_14h"},
        {37,"Tomorrow_15h"},
        {38,"Tomorrow_16h"}
    }.ToImmutableDictionary();

    public static readonly ImmutableDictionary<int, string> ExtensionCode = new Dictionary<int, string>
    {
        {1,"REAL"},
        {2,"SPECIAL"}
    }.ToImmutableDictionary();

    public static readonly string[] ManagementRole = { "Section Head", "Management", "Supervisor", "Leader", "Assistant Leader", "Unit Head" };
}

public static class OrderConstant
{
    public static readonly string[] MainMatchLevel = { "1st", "2nd", "[OP]1st", "[OP]2nd", "Only SB", null };

    public static readonly string[] ChildMatchLevel = { "[OP]1st", "[OP]2nd", "1st", "2nd", "Only SB", null };

    public static readonly string[] RoleOrder = { "Staff", "Assistant Leader", "Leader", "Supervisor", "Section Head", "Management", "Unit Head", null };

    public const string BetType2 = "2";

    public const string BetType2Hdp = "2[HDP]";

    public const string BetType2Ou = "2[OU]";

    public const string BetTypeFs = "FS";

    public const string BetType2Op = "2[OP]";

    public const string BetTypeSb = "SB";

    public const string BetType5Min = "5min";

    public const string BetType6Min = "6min";

    public const string BetTypeBkCr = "BK CR";

    public static readonly string[] MainBetTypesHdp = { BetType2, BetType2Hdp, BetType2Ou, BetTypeFs, BetType2Op, BetTypeBkCr, BetTypeSb, BetType5Min, BetType6Min };

    public static readonly string[] MainBetTypesOu = { BetType2, BetType2Ou, BetType2Hdp, BetTypeFs, BetType2Op, BetTypeBkCr, BetTypeSb, BetType5Min, BetType6Min };

    public static readonly string[] MainBetTypesFs = { BetTypeFs, BetType2, BetType2Hdp, BetType2Ou, BetType2Op, BetTypeBkCr, BetTypeSb, BetType5Min, BetType6Min };

    public static readonly string[] MainBetTypesChild = { BetType2Op, BetTypeBkCr, BetType2, BetType2Ou, BetType2Hdp, BetTypeFs, BetTypeSb, BetType5Min, BetType6Min };

    public static readonly int[] HdpTypeIds = { 1, 7 };

    public static readonly int[] OuTypeIds = { 3, 8 };

    public const decimal LowerMaxBet = 35_000;

    public const decimal UpperMaxBet = 80_000;
}

public static class MatchTypeLabel
{
	public static ImmutableDictionary<int, string> MatchTypeMapLabel = ImmutableDictionary.CreateRange(
		new[]
		{
			KeyValuePair.Create((int)MatchType.Saba5Mins, "5"),
			KeyValuePair.Create((int)MatchType.Saba15Mins, "15"),
			KeyValuePair.Create((int)MatchType.Saba16Mins, "16"),
			KeyValuePair.Create((int)MatchType.Saba20Mins, "20"),
		}
	);
}




