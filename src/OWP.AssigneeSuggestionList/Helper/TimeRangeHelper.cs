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
            bool hasEtpk)
        {
            var increaseHour = INCREASE_HOUR_DEFAULT;
            var increaseHourPair = new Dictionary<string, int>();

            var hdp = (int)EnumBettype.Handicap;
            var ou = (int)EnumBettype.OverUnder;
            var fhHdp = (int)EnumBettype.FirstHalfHandicap;
            var fhOu = (int)EnumBettype.FirstHalfOverUnder;

            foreach (var bettypeId in bettypeIds)
            {
                if ((bettypeId == hdp || bettypeId == ou))
                {
                    increaseHourPair.TryAdd("FT", INCREASE_HOUR_BY_TWO);
                }
                else if (bettypeId == fhHdp || bettypeId == fhOu)
                {
                    increaseHourPair.TryAdd("FH", INCREASE_HOUR_BY_ONE);
                }
                else
                {

                }
            }

            if (hasEtpk)
            {
                increaseHourPair.TryAdd("ET_PK", INCREASE_HOUR_BY_ONE);
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