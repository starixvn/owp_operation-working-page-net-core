namespace OWP.OtherSport.Notification.Extensions
{
    using System.Collections.Generic;
    using Constants;
    using Validation.Constants;

    public static class BettypeNameExtensions
    {
        public static string GetBettypeNameForAssignment(int bettypeId, int minorBettypeId, string minorBettypeName)
        {
            var minorGenerateList = new List<int> {
                (int)MinorBettype.Specific15Mins,
                (int)MinorBettype.Specific10Mins,
                (int)MinorBettype.FastMarket,
                (int)MinorBettype.AllInOne,
                (int)MinorBettype.Specific15MinsCorners,
                (int)MinorBettype.Specific15MinsTotalBookings
            };

            if (minorBettypeId == 0)
            {
                return MajorBettypeShortName.GetMajorBetTypeShortName(bettypeId);
            }

            return minorGenerateList.Contains(minorBettypeId)
                ? minorBettypeName
                : $"{minorBettypeName} ({MajorBettypeShortName.GetMajorBetTypeShortName(bettypeId)})";
        }
    }
}