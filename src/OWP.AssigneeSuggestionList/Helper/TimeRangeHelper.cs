using OWP.Enums;

namespace OWP.AssigneeSuggestionList.Helper;

public static class TimeRangeHelper
{
    private const int INCREASE_HOUR_BY_TWO = 2;

    private const int INCREASE_HOUR_BY_ONE = 1;

    public static (DateTime startTime, DateTime endTime) CalculateTimeRangeFT(DateTime eventDate, bool hasEtpk)
        => hasEtpk
            ? (eventDate, eventDate.AddHours(INCREASE_HOUR_BY_TWO))
            : (eventDate, eventDate.AddHours(INCREASE_HOUR_BY_ONE));

    public static (DateTime startTime, DateTime endTime) CalculateTimeRangeSingleGame(
        DateTime eventDate,
        IList<int> bettypeIds,
        bool hasEtpk
    )
    {
        var increaseHourPair = new Dictionary<string, int>();

        const int HDP = (int)EnumBettype.Handicap;
        const int OU = (int)EnumBettype.OverUnder;

        foreach (var bettypeId in bettypeIds)
        {
            switch (bettypeId)
            {
                case HDP or OU:
                    increaseHourPair.TryAdd("FT", INCREASE_HOUR_BY_ONE);
                    break;
                default:
                    continue;
            }
        }

        if (hasEtpk)
        {
            increaseHourPair.TryAdd("ET_PK", INCREASE_HOUR_BY_ONE);
        }

        var increaseHour = increaseHourPair.Values.Sum();
        var endDate = eventDate.AddHours(increaseHour);
        return (eventDate, endDate);
    }
}