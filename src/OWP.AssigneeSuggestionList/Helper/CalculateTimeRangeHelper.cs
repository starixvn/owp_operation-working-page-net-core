using OWP.AssigneeSuggestionList.Models;
using OWP.Enums;

namespace OWP.AssigneeSuggestionList.Helper
{
    public static class CalculateTimeRangeHelper
    {
        private const int INCREASE_HOUR_DEFAULT = 0;

        private const int INCREASE_HOUR_BY_TWO = 2;

        private const int INCREASE_HOUR_BY_ONE = 1;

        public static (DateTime startTime, DateTime endTime) CalculateTimeRange(ScheduleUpdateModel updateModel)
        {
            var increaseHour = INCREASE_HOUR_DEFAULT;
            var increaseHourPair = new Dictionary<string, int>();

            var hdp = (int)EnumBettype.Handicap;
            var ou = (int)EnumBettype.OverUnder;
            var fhHdp = (int)EnumBettype.FirstHalfHandicap;
            var fhOu = (int)EnumBettype.FirstHalfOverUnder;

            foreach (var bettypeId in updateModel.BettypeIds)
            {
                if (!updateModel.Is15Mins
                 && (bettypeId == hdp
                || bettypeId == ou))
                {
                    increaseHourPair.TryAdd("FT", INCREASE_HOUR_BY_TWO);
                }
                else if (updateModel.Is15Mins
                || bettypeId == fhHdp
                || bettypeId == fhOu)
                {
                    increaseHourPair.TryAdd("FIRST_HALF", INCREASE_HOUR_BY_ONE);
                }
                else
                {

                }
            }

            if (updateModel.EtPk.IsHasEtPk)
            {
                increaseHourPair.TryAdd("ET_PK", INCREASE_HOUR_BY_ONE);
            }

            foreach (var value in increaseHourPair.Values)
            {
                increaseHour += value;
            }
            var endDate = updateModel.EventDate.AddDays(increaseHour);
            return (updateModel.EventDate, endDate);
        }
    }
}