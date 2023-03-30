using OWP.Enums;

namespace OWP.AssigneeSuggestionList.Helper
{
    public static class TimeRangeHelper
    {
        private const int INCREASE_HOUR_DEFAULT = 0;

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
            var increaseHour = INCREASE_HOUR_DEFAULT;
            var increaseHourPair = new Dictionary<string, int>();

            const int HDP = (int)EnumBettype.Handicap;
            const int OU = (int)EnumBettype.OverUnder;
            const int FT_HDP = (int)EnumBettype.FirstHalfHandicap;
            const int FT_OU = (int)EnumBettype.FirstHalfOverUnder;

            foreach (var bettypeId in bettypeIds)
            {
                if ((bettypeId == HDP || bettypeId == OU))
                {
                    increaseHourPair.TryAdd("FT", INCREASE_HOUR_BY_TWO);
                }
                if (bettypeId == FT_HDP || bettypeId == FT_OU)
                {
                    increaseHourPair.TryAdd("FH", INCREASE_HOUR_BY_ONE);
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
            foreach (var value in increaseHourPair.Values)
            {
                increaseHour += value;
            }
            var endDate = eventDate.AddDays(increaseHour);
            return (eventDate, endDate);
        }
    }
}