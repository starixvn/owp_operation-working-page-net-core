namespace OWP.AssignmentUpload.Validation.Services
{
    using System.Collections.Generic;
    using Constants;
    using Models;

    public interface ImplicitMinorMatchGenerater
    {
        IEnumerable<AssignmentModel> GenerateImplicitMinorMatchForExplicitMinorMatch(AssignmentModel mainMatch);

        IEnumerable<AssignmentModel> GenerateImplicitMinorMatchForMainMatch(AssignmentModel mainMatch);

        List<AssignmentModel> GenerateMinorMatch(List<AssignmentModel> assignments);

        AssignmentModel GetAllInOneMatch(AssignmentModel mainMatch);

        AssignmentModel GetFastMarketMatch(AssignmentModel mainMatch);

        AssignmentModel GetSpecific10MinsMatch(AssignmentModel mainMatch);

        AssignmentModel GetSpecific15MinsCornersMatch(AssignmentModel mainMatch);

        AssignmentModel GetSpecific15MinsMatch(AssignmentModel mainMatch);

        AssignmentModel GetSpecific15MinsTotalBookingsMatch(AssignmentModel mainMatch);
    }

    public class GenerateImplicitMinorMatchImp : ImplicitMinorMatchGenerater
    {
        private const string SPECIFIC_15_MINS = " (SPECIFIC 15 MINS)";
        private const string SPECIFIC_10_MINS = " (SPECIFIC 10 MINS)";

        public List<AssignmentModel> GenerateMinorMatch(List<AssignmentModel> assignments)
        {
            var newAssignments = new List<AssignmentModel>();
            foreach (var match in assignments)
            {
                newAssignments.Add(match);
                var minorMatches = string.IsNullOrEmpty(match.ParentHashKey)
                    ? GenerateImplicitMinorMatchForMainMatch(match)
                    : GenerateImplicitMinorMatchForExplicitMinorMatch(match);
                newAssignments.AddRange(minorMatches);
            }

            return newAssignments;
        }

        public IEnumerable<AssignmentModel> GenerateImplicitMinorMatchForMainMatch(AssignmentModel mainMatch)
        {
            yield return GetSpecific15MinsMatch(mainMatch);
            yield return GetSpecific10MinsMatch(mainMatch);
            yield return GetFastMarketMatch(mainMatch);
            yield return GetAllInOneMatch(mainMatch);
        }

        public AssignmentModel GetAllInOneMatch(AssignmentModel mainMatch)
        {
            return new AssignmentModel
            {
                MinorBettypeId = (int)MinorBettype.AllInOne,
                SessionId = mainMatch.SessionId,
                HashKey = mainMatch.HashKey + MinorBettype.AllInOne,
                IsImplicitMinorMatch = true,
                MatchStatus = mainMatch.MatchStatus,
                ParentHashKey = mainMatch.HashKey,
                EventDate = mainMatch.EventDate,
                KickOffTime = mainMatch.KickOffTime,
                OtherTime = mainMatch.OtherTime,
                League = mainMatch.League + " - ALL IN ONE",
                Home = mainMatch.Home + " All In One",
                Away = mainMatch.Away + " All In One",

                TraderHDP = mainMatch.TraderAIO,
                OtherTradersHDP = mainMatch.OtherTradersAIO,
                IsSaba = mainMatch.IsSaba
            };
        }

        public AssignmentModel GetFastMarketMatch(AssignmentModel mainMatch)
        {
            return new AssignmentModel
            {
                MinorBettypeId = (int)MinorBettype.FastMarket,
                SessionId = mainMatch.SessionId,
                HashKey = mainMatch.HashKey + MinorBettype.FastMarket,
                IsImplicitMinorMatch = true,
                MatchStatus = mainMatch.MatchStatus,
                ParentHashKey = mainMatch.HashKey,
                EventDate = mainMatch.EventDate,
                KickOffTime = mainMatch.KickOffTime,
                OtherTime = mainMatch.OtherTime,
                League = mainMatch.League + " - FAST MARKET",
                Home = mainMatch.Home + " Fast Market",
                Away = mainMatch.Away + " Fast Market",
                MaxBetHDP = mainMatch.MaxBetFM,
                SpreadHDP = mainMatch.SpreadFM,
                TraderHDP = mainMatch.TraderFM,
                OtherTradersHDP = mainMatch.OtherTradersFM,
                IsSaba = mainMatch.IsSaba
            };
        }

        public AssignmentModel GetSpecific10MinsMatch(AssignmentModel mainMatch)
        {
            return new AssignmentModel
            {
                MinorBettypeId = (int)MinorBettype.Specific10Mins,
                SessionId = mainMatch.SessionId,
                HashKey = mainMatch.HashKey + MinorBettype.Specific10Mins,
                IsImplicitMinorMatch = true,
                MatchStatus = mainMatch.MatchStatus,
                ParentHashKey = mainMatch.HashKey,
                EventDate = mainMatch.EventDate,
                KickOffTime = mainMatch.KickOffTime,
                OtherTime = mainMatch.OtherTime,
                League = mainMatch.League + " - SPECIFIC 10 MINS",
                Home = mainMatch.Home + SPECIFIC_10_MINS,
                Away = mainMatch.Away + SPECIFIC_10_MINS,

                MaxBetHDP = mainMatch.MaxBetXMins,
                SpreadHDP = mainMatch.SpreadXMins,
                TraderHDP = mainMatch.Trader10Mins,
                OtherTradersHDP = mainMatch.OtherTraders10Mins,
                IsSaba = mainMatch.IsSaba
            };
        }

        public AssignmentModel GetSpecific15MinsMatch(AssignmentModel mainMatch)
        {
            var minorMatch = GetSpecific15MinsMatchCommonFields(mainMatch);
            minorMatch.MinorBettypeId = (int)MinorBettype.Specific15Mins;
            minorMatch.HashKey = mainMatch.HashKey + MinorBettype.Specific15Mins;
            minorMatch.League = mainMatch.League + " - SPECIFIC 15 MINS";
            minorMatch.ParentHashKey = mainMatch.HashKey;
            minorMatch.IsSaba = mainMatch.IsSaba;

            return minorMatch;
        }

        public IEnumerable<AssignmentModel> GenerateImplicitMinorMatchForExplicitMinorMatch(AssignmentModel mainMatch)
        {
            if (Equals(mainMatch.MinorBettypeId, (int)MinorBettype.NoOfCorner))
            {
                yield return GetSpecific15MinsCornersMatch(mainMatch);
            }

            if (Equals(mainMatch.MinorBettypeId, (int)MinorBettype.TotalBooking))
            {
                yield return GetSpecific15MinsTotalBookingsMatch(mainMatch);
            }
        }

        public AssignmentModel GetSpecific15MinsTotalBookingsMatch(AssignmentModel mainMatch)
        {
            var minorMatch = GetSpecific15MinsMatchCommonFields(mainMatch);
            minorMatch.MinorBettypeId = (int)MinorBettype.Specific15MinsTotalBookings;
            minorMatch.HashKey = mainMatch.HashKey + MinorBettype.Specific15MinsTotalBookings;
            minorMatch.League = mainMatch.League.Replace(" - BOOKING", " - SPECIFIC 15 MINS TOTAL BOOKINGS");
            minorMatch.ParentHashKey = mainMatch.ParentHashKey;

            return minorMatch;
        }

        public AssignmentModel GetSpecific15MinsCornersMatch(AssignmentModel mainMatch)
        {
            var minorMatch = GetSpecific15MinsMatchCommonFields(mainMatch);
            minorMatch.MinorBettypeId = (int)MinorBettype.Specific15MinsCorners;
            minorMatch.HashKey = mainMatch.HashKey + MinorBettype.Specific15MinsCorners;
            minorMatch.League = mainMatch.League.Replace(" - CORNERS", " - SPECIFIC 15 MINS NUMBER OF CORNERS");
            minorMatch.ParentHashKey = mainMatch.ParentHashKey;

            return minorMatch;
        }

        private static AssignmentModel GetSpecific15MinsMatchCommonFields(AssignmentModel mainMatch)
        {
            return new AssignmentModel
            {
                SessionId = mainMatch.SessionId,
                IsImplicitMinorMatch = true,
                MatchStatus = mainMatch.MatchStatus,
                EventDate = mainMatch.EventDate,
                KickOffTime = mainMatch.KickOffTime,
                OtherTime = mainMatch.OtherTime,
                Home = mainMatch.Home + SPECIFIC_15_MINS,
                Away = mainMatch.Away + SPECIFIC_15_MINS,

                MaxBetHDP = mainMatch.MaxBetXMins,
                SpreadHDP = mainMatch.SpreadXMins,
                TraderHDP = mainMatch.Trader15Mins,
                OtherTradersHDP = mainMatch.OtherTraders15Mins
            };
        }
    }
}