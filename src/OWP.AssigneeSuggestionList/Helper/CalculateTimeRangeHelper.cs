using OWP.Enums;

namespace OWP.AssigneeSuggestionList.Helper
{
    public static class CalculateTimeRangeHelper
    {
        private const int INCREASE_HOUR_DEFAULT = 0;
        private const int INCREASE_HOUR_BY_TWO = 2;
        private const int INCREASE_HOUR_BY_ONE = 1;

        public static (DateTime, DateTime) CalculateTimeRange(DateTime eventDate, IList<int> bettypeIds, bool isEtPK)
        {
            var increaseHour = INCREASE_HOUR_DEFAULT;
            var increaseHourPair = new Dictionary<string, int>();
            foreach (var bettypeId in bettypeIds)
            {
                if (bettypeId == (int)EnumBettype.Handicap
                || bettypeId == (int)EnumBettype.OverUnder)
                {
                    increaseHourPair.TryAdd("FT", INCREASE_HOUR_BY_TWO);
                }
                else if (bettypeId == (int)EnumBettype.FirstHalfHandicap
                || bettypeId == (int)EnumBettype.FirstHalfOverUnder)
                {
                    increaseHourPair.TryAdd("FIRST_HALF", INCREASE_HOUR_BY_ONE);
                }
                else
                {

                }
            }

            if (isEtPK)
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