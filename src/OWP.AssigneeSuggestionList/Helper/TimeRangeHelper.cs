using OWP.Enums;

namespace OWP.AssigneeSuggestionList.Helper;

public static class TimeRangeHelper
{
    private const int INCREASE_HOUR_BY_TWO = 2;

    private const int INCREASE_HOUR_BY_ONE = 1;

    private const int INCREASE_HOUR_BY_THREE = 3;

    public static (DateTime startTime, DateTime endTime) CalculateTimeRangeFT(DateTime eventDate, bool hasEtpk)
        => hasEtpk
            ? (eventDate, eventDate.AddHours(INCREASE_HOUR_BY_THREE))
            : (eventDate, eventDate.AddHours(INCREASE_HOUR_BY_TWO));

    public static (DateTime startTime, DateTime endTime) CalculateTimeRangeSingleGame(
        DateTime eventDate,
        IList<int> bettypeIds,
        bool hasEtpk
    )
    {
        var increaseHourPair = new Dictionary<string, int>();

        const int HDP = (int)EnumBettype.Handicap;
        const int OU = (int)EnumBettype.OverUnder;
        const int FT_HDP = (int)EnumBettype.FirstHalfHandicap;
        const int FT_OU = (int)EnumBettype.FirstHalfOverUnder;

        foreach (var bettypeId in bettypeIds)
        {
            switch (bettypeId)
            {
                case HDP:
                case OU:
                    increaseHourPair.TryAdd("FT", INCREASE_HOUR_BY_TWO);
                    break;
                case FT_HDP:
                case FT_OU:
                    increaseHourPair.TryAdd("FH", INCREASE_HOUR_BY_ONE);
                    break;
                default: break;
            }
        }

        if (hasEtpk)
        {
            increaseHourPair.TryAdd("ET_PK", INCREASE_HOUR_BY_ONE);
        }

        if (increaseHourPair.ContainsKey("FT"))
        {
            increaseHourPair.Remove("FH");
        }

        var increaseHour = increaseHourPair.Values.Sum();
        var endDate = eventDate.AddHours(increaseHour);
        return (eventDate, endDate);
    }
}