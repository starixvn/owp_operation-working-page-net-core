namespace OWP.OSLeagueSetting.Constants;

using System.Collections.Immutable;

public static class LeagueSettingConst
{
    public const string ManualUpdate = "Manual Update Score";

    public const string IpTv = "IPTV Only";

    public const string OfferLines = "OfferLines";

    public const string BetType = "BetType";

    public const string Remark = "Remark";

    public const string TimeRange = "TimeRange";

    public const string BetDelay = "BetDelay";

    public const string WeekMatch = "WeekMatch";

    public const string Provider = "Provider";

    public const string ManualUpdateStyle = "update-score";

    public const string IpTvStyle = "Iptv-only";

    public const string OfferLinesStyle = "offer-lines";

    public const string BetTypeStyle = "bet-type";

    public const string RemarkStyle = "remark";

    public const string TimeRangeStyle = "time-range";

    public const string BetDelayStyle = "bet-delay";

    public const string WeekMatchStyle = "week-match";

    public const string ProviderStyle = "provider";

    public const int TotalProvider = 3;

    public const int TotalOfferLines = 10;

    public const int TotalBetDelay = 5;

    public const int TotalWeekMatch = 27;

    public static readonly ImmutableDictionary<string, int> SummaryFullOption = new Dictionary<string, int>
    {
        {
            OfferLines,
            TotalOfferLines
        },
        { BetDelay, TotalBetDelay },
        { WeekMatch, TotalWeekMatch },
        { Provider, TotalProvider }
    }.ToImmutableDictionary();

    public static readonly ImmutableDictionary<string, string> SummaryStyleOption = new Dictionary<string, string>
    {
        {
            OfferLines,
            OfferLinesStyle
        },
        { BetDelay, BetDelayStyle },
        { WeekMatch, WeekMatchStyle },
        { Provider, ProviderStyle },
        {ManualUpdate,ManualUpdateStyle},
        {IpTv,IpTvStyle},
        {BetType,BetTypeStyle},
        {Remark,RemarkStyle},
        {TimeRange,TimeRangeStyle}
    }.ToImmutableDictionary();
}

public enum DuplicateType
{
    Round,
    OfferLine,
    BetDelay,
    WeekMatch,
    Provider,
    RangeTime,
    IPTV,
    Remark
}