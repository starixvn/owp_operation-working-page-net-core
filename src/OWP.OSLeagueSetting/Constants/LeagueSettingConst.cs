namespace OWP.OSLeagueSetting.Constants;

public static class LeagueSettingConst
{
    public const string ManualUpdate = "Manual Update Score";

    public const string IPTV = "IPTV Only";

    public const string OfferLines = "OfferLines";

    public const int TotalOfferLines = 9;

    public const int NumberBetType = 16;

    public const string BetDelay = "Bet Delay";

    public const int TotalBetDelay = 4;

    public const string WeekMatch = "Week Match";

    public const int TotalWeekMatch = 26;

    public const string Provider = "Provider";

    public const int TotalProvider = 3;

    public static readonly Dictionary<string, int> SummaryFullOption = new()
    {
        {
            OfferLines,
            TotalOfferLines
        },
        { BetDelay, TotalBetDelay },
        { WeekMatch, TotalWeekMatch },
        { Provider, TotalProvider }
    };
}