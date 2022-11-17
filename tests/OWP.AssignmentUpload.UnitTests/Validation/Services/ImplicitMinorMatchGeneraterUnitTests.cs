namespace OWP.AssignmentUpload.UnitTests.Validation.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AssignmentUpload.Validation.Models;
    using AssignmentUpload.Validation.Services;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class GenerateImplicitMinorMatchUnitTests
    {
        private readonly GenerateImplicitMinorMatchImp generateImplicitMinorMatch;

        public GenerateImplicitMinorMatchUnitTests()
        {
            generateImplicitMinorMatch = new GenerateImplicitMinorMatchImp();
        }

        [Fact]
        public void GetSpecific15MinsMatch_MainMatch_ReturnSpecific15MinsMatch()
        {
            var mainMatch = new AssignmentModel
            {
                League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS",
                Home = "Ha Noi FC U19 (N)",
                Away = "Than Quang Ninh U19",

                SessionId = "sesionId1234",
                HashKey = "hashkey12345",
                MatchStatus = "Live",
                EventDate = new DateTime(2020, 08, 19),
                KickOffTime = new TimeSpan(0, 15, 0, 0),
                MaxBetXMins = 5000,
                SpreadXMins = (decimal)0.5,
                Trader15Mins = 8,
                OtherTraders15Mins = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
            };
            var expected = new AssignmentModel
            {
                MinorBettypeId = -1,
                ParentHashKey = "hashkey12345",
                SessionId = "sesionId1234",
                MatchStatus = "Live",
                EventDate = new DateTime(2020, 08, 19),
                KickOffTime = new TimeSpan(0, 15, 0, 0),
                IsImplicitMinorMatch = true,

                League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS - SPECIFIC 15 MINS",
                Home = "Ha Noi FC U19 (N) (SPECIFIC 15 MINS)",
                Away = "Than Quang Ninh U19 (SPECIFIC 15 MINS)",

                MaxBetHDP = 5000,
                SpreadHDP = (decimal)0.5,
                TraderHDP = 8,
                OtherTradersHDP = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
            };

            var actual = generateImplicitMinorMatch.GetSpecific15MinsMatch(mainMatch);
            DeepAssert.Equal(expected, actual, "HashKey");
            Assert.False(string.IsNullOrEmpty(actual.HashKey));
        }

        [Fact]
        public void GetSpecific10MinsMatch_MainMatch_ReturnSpecific10MinsMatch()
        {
            var mainMatch = new AssignmentModel
            {
                League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS",
                Home = "Ha Noi FC U19 (N)",
                Away = "Than Quang Ninh U19",

                SessionId = "sesionId1234",
                HashKey = "hashkey12345",
                MatchStatus = "Live",
                EventDate = new DateTime(2020, 08, 19),
                KickOffTime = new TimeSpan(0, 15, 0, 0),
                MaxBetXMins = 5000,
                SpreadXMins = (decimal)0.5,
                Trader10Mins = 8,
                OtherTraders10Mins = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
            };
            var expected = new AssignmentModel
            {
                MinorBettypeId = -2,
                ParentHashKey = "hashkey12345",
                SessionId = "sesionId1234",

                MatchStatus = "Live",
                EventDate = new DateTime(2020, 08, 19),
                KickOffTime = new TimeSpan(0, 15, 0, 0),
                IsImplicitMinorMatch = true,

                League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS - SPECIFIC 10 MINS",
                Home = "Ha Noi FC U19 (N) (SPECIFIC 10 MINS)",
                Away = "Than Quang Ninh U19 (SPECIFIC 10 MINS)",

                MaxBetHDP = 5000,
                SpreadHDP = (decimal)0.5,
                TraderHDP = 8,
                OtherTradersHDP = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
            };

            var actual = generateImplicitMinorMatch.GetSpecific10MinsMatch(mainMatch);
            DeepAssert.Equal(expected, actual, "HashKey");
            Assert.False(string.IsNullOrEmpty(actual.HashKey));
        }

        [Fact]
        public void GetFastMarketMatch_MainMatch_ReturnFastMarketMatch()
        {
            var mainMatch = new AssignmentModel
            {
                League = "League",
                Home = "Home",
                Away = "Away",

                SessionId = "sesionId1234",
                HashKey = "hashkey12345",
                MatchStatus = "Live",
                EventDate = new DateTime(2020, 08, 19),
                KickOffTime = new TimeSpan(0, 15, 0, 0),
                MaxBetFM = 5000,
                SpreadFM = (decimal)0.5,
                TraderFM = 8,
                OtherTradersFM = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
            };
            var expected = new AssignmentModel
            {
                MinorBettypeId = -88,
                ParentHashKey = "hashkey12345",
                SessionId = "sesionId1234",

                MatchStatus = "Live",
                EventDate = new DateTime(2020, 08, 19),
                KickOffTime = new TimeSpan(0, 15, 0, 0),
                IsImplicitMinorMatch = true,

                League = "League - FAST MARKET",
                Home = "Home Fast Market",
                Away = "Away Fast Market",

                MaxBetHDP = 5000,
                SpreadHDP = (decimal)0.5,
                TraderHDP = 8,
                OtherTradersHDP = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
            };

            var actual = generateImplicitMinorMatch.GetFastMarketMatch(mainMatch);
            DeepAssert.Equal(expected, actual, "HashKey");
            Assert.False(string.IsNullOrEmpty(actual.HashKey));
        }

        [Fact]
        public void GetAllInOneMatch_MainMatch_ReturnAllInOneMatch()
        {
            var mainMatch = new AssignmentModel
            {
                League = "League",
                Home = "Home",
                Away = "Away",

                SessionId = "sesionId1234",
                HashKey = "hashkey12345",
                MatchStatus = "Live",
                EventDate = new DateTime(2020, 08, 19),
                KickOffTime = new TimeSpan(0, 15, 0, 0),

                TraderAIO = 8,
                OtherTradersAIO = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
            };
            var expected = new AssignmentModel
            {
                MinorBettypeId = -89,
                ParentHashKey = "hashkey12345",
                SessionId = "sesionId1234",

                MatchStatus = "Live",
                EventDate = new DateTime(2020, 08, 19),
                KickOffTime = new TimeSpan(0, 15, 0, 0),
                IsImplicitMinorMatch = true,

                League = "League - ALL IN ONE",
                Home = "Home All In One",
                Away = "Away All In One",

                TraderHDP = 8,
                OtherTradersHDP = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
            };

            var actual = generateImplicitMinorMatch.GetAllInOneMatch(mainMatch);
            DeepAssert.Equal(expected, actual, "HashKey");
            Assert.False(string.IsNullOrEmpty(actual.HashKey));
        }

        [Fact]
        public void GetSpecific15MinsCornersMatch_MinorMatch_ReturnSpecific15MinsCornersMatch()
        {
            var minorMatch = new AssignmentModel
            {
                League = "League - CORNERS",
                Home = "Home No.of Corners",
                Away = "Away No.of Corners",

                SessionId = "sesionId1234",
                ParentHashKey = "parentHashkey12",
                HashKey = "hashkey12345",
                MatchStatus = "Live",
                EventDate = new DateTime(2020, 08, 19),
                KickOffTime = new TimeSpan(0, 15, 0, 0),

                MaxBetXMins = 5000,
                SpreadXMins = (decimal)0.5,
                Trader15Mins = 8,
                OtherTraders15Mins = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
            };
            var expected = new AssignmentModel
            {
                MinorBettypeId = -5,
                ParentHashKey = "parentHashkey12",
                SessionId = "sesionId1234",

                MatchStatus = "Live",
                EventDate = new DateTime(2020, 08, 19),
                KickOffTime = new TimeSpan(0, 15, 0, 0),
                IsImplicitMinorMatch = true,

                League = "League - SPECIFIC 15 MINS NUMBER OF CORNERS",
                Home = "Home No.of Corners (SPECIFIC 15 MINS)",
                Away = "Away No.of Corners (SPECIFIC 15 MINS)",
                MaxBetHDP = 5000,
                SpreadHDP = (decimal)0.5,
                TraderHDP = 8,
                OtherTradersHDP = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
            };

            var actual = generateImplicitMinorMatch.GetSpecific15MinsCornersMatch(minorMatch);
            DeepAssert.Equal(expected, actual, "HashKey");
            Assert.False(string.IsNullOrEmpty(actual.HashKey));
        }

        [Fact]
        public void GetSpecific15MinsTotalBookingsMatch_MinorMatch_ReturnSpecific15MinsTotalBookingsMatch()
        {
            var minorMatch = new AssignmentModel
            {
                League = "League - BOOKING",
                Home = "Home Total Bookings",
                Away = "Away Total Bookings",

                SessionId = "sesionId1234",
                ParentHashKey = "parentHashkey12",
                HashKey = "hashkey12345",
                MatchStatus = "Live",
                EventDate = new DateTime(2020, 08, 19),
                KickOffTime = new TimeSpan(0, 15, 0, 0),

                MaxBetXMins = 5000,
                SpreadXMins = (decimal)0.5,
                Trader15Mins = 8,
                OtherTraders15Mins = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
            };
            var expected = new AssignmentModel
            {
                MinorBettypeId = -8,
                ParentHashKey = "parentHashkey12",
                SessionId = "sesionId1234",

                MatchStatus = "Live",
                EventDate = new DateTime(2020, 08, 19),
                KickOffTime = new TimeSpan(0, 15, 0, 0),
                IsImplicitMinorMatch = true,

                League = "League - SPECIFIC 15 MINS TOTAL BOOKINGS",
                Home = "Home Total Bookings (SPECIFIC 15 MINS)",
                Away = "Away Total Bookings (SPECIFIC 15 MINS)",
                MaxBetHDP = 5000,
                SpreadHDP = (decimal)0.5,
                TraderHDP = 8,
                OtherTradersHDP = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
            };

            var actual = generateImplicitMinorMatch.GetSpecific15MinsTotalBookingsMatch(minorMatch);
            DeepAssert.Equal(expected, actual, "HashKey");
            Assert.False(string.IsNullOrEmpty(actual.HashKey));
        }

        [Fact]
        public void GenerateImplicitMinorMatchForMainMatch_OneMainMatch_Return4Match()
        {
            var mainMatch = new AssignmentModel
            {
                League = "League",
                Home = "Home",
                Away = "Away",

                SessionId = "sesionId1234",
                ParentHashKey = "parentHashkey12",
                HashKey = "hashkey12345",
                MatchStatus = "Live",
                EventDate = new DateTime(2020, 08, 19),
                KickOffTime = new TimeSpan(0, 15, 0, 0),

                MaxBetXMins = 5000,
                SpreadXMins = (decimal)0.5,
                Trader15Mins = 8,
                OtherTraders15Mins = "<Root><r u=\"0\" n=\"starix1\" /></Root>",
                Trader10Mins = 8,
                OtherTraders10Mins = "<Root><r u=\"0\" n=\"starix1\" /></Root>",
                MaxBetFM = 5000,
                SpreadFM = (decimal)0.5,
                TraderFM = 8,
                OtherTradersFM = "<Root><r u=\"0\" n=\"starix1\" /></Root>",
                TraderAIO = 8,
                OtherTradersAIO = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
            };

            var expected = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    MinorBettypeId = -1,
                    ParentHashKey = "hashkey12345",
                    SessionId = "sesionId1234",

                    MatchStatus = "Live",
                    EventDate = new DateTime(2020, 08, 19),
                    KickOffTime = new TimeSpan(0, 15, 0, 0),
                    IsImplicitMinorMatch = true,

                    League = "League - SPECIFIC 15 MINS",
                    Home = "Home (SPECIFIC 15 MINS)",
                    Away = "Away (SPECIFIC 15 MINS)",

                    MaxBetHDP = 5000,
                    SpreadHDP = (decimal)0.5,
                    TraderHDP = 8,
                    OtherTradersHDP = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
                },
                new AssignmentModel
                {
                    MinorBettypeId = -2,
                    ParentHashKey = "hashkey12345",
                    SessionId = "sesionId1234",

                    MatchStatus = "Live",
                    EventDate = new DateTime(2020, 08, 19),
                    KickOffTime = new TimeSpan(0, 15, 0, 0),
                    IsImplicitMinorMatch = true,

                    League = "League - SPECIFIC 10 MINS",
                    Home = "Home (SPECIFIC 10 MINS)",
                    Away = "Away (SPECIFIC 10 MINS)",

                    MaxBetHDP = 5000,
                    SpreadHDP = (decimal)0.5,
                    TraderHDP = 8,
                    OtherTradersHDP = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
                },
                new AssignmentModel
                {
                    MinorBettypeId = -88,
                    ParentHashKey = "hashkey12345",
                    SessionId = "sesionId1234",

                    MatchStatus = "Live",
                    EventDate = new DateTime(2020, 08, 19),
                    KickOffTime = new TimeSpan(0, 15, 0, 0),
                    IsImplicitMinorMatch = true,

                    League = "League - FAST MARKET",
                    Home = "Home Fast Market",
                    Away = "Away Fast Market",

                    MaxBetHDP = 5000,
                    SpreadHDP = (decimal)0.5,
                    TraderHDP = 8,
                    OtherTradersHDP = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
                },
                new AssignmentModel
                {
                    MinorBettypeId = -89,
                    ParentHashKey = "hashkey12345",
                    SessionId = "sesionId1234",

                    MatchStatus = "Live",
                    EventDate = new DateTime(2020, 08, 19),
                    KickOffTime = new TimeSpan(0, 15, 0, 0),
                    IsImplicitMinorMatch = true,

                    League = "League - ALL IN ONE",
                    Home = "Home All In One",
                    Away = "Away All In One",

                    TraderHDP = 8,
                    OtherTradersHDP = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
                }
            };

            var actual = generateImplicitMinorMatch.GenerateImplicitMinorMatchForMainMatch(mainMatch).ToList();
            DeepAssert.Equal(expected, actual, "HashKey");
            Assert.False(string.IsNullOrEmpty(actual[0].HashKey));
            Assert.False(string.IsNullOrEmpty(actual[1].HashKey));
            Assert.False(string.IsNullOrEmpty(actual[2].HashKey));
            Assert.False(string.IsNullOrEmpty(actual[3].HashKey));
        }

        [Fact]
        public void GenerateImplicitMinorMatchForExplicitMinorMatch_MinorMatch_Return()
        {
            var minorMatch = new AssignmentModel
            {
                MinorBettypeId = -3,
                League = "League - CORNERS",
                Home = "Home No.of Corners",
                Away = "Away No.of Corners",

                SessionId = "sesionId1234",
                ParentHashKey = "parentHashkey12",
                HashKey = "hashkey12345",
                MatchStatus = "Live",
                EventDate = new DateTime(2020, 08, 19),
                KickOffTime = new TimeSpan(0, 15, 0, 0),

                MaxBetXMins = 5000,
                SpreadXMins = (decimal)0.5,
                Trader15Mins = 8,
                OtherTraders15Mins = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
            };
            var expected = new AssignmentModel
            {
                MinorBettypeId = -5,
                ParentHashKey = "parentHashkey12",
                SessionId = "sesionId1234",

                MatchStatus = "Live",
                EventDate = new DateTime(2020, 08, 19),
                KickOffTime = new TimeSpan(0, 15, 0, 0),
                IsImplicitMinorMatch = true,

                League = "League - SPECIFIC 15 MINS NUMBER OF CORNERS",
                Home = "Home No.of Corners (SPECIFIC 15 MINS)",
                Away = "Away No.of Corners (SPECIFIC 15 MINS)",
                MaxBetHDP = 5000,
                SpreadHDP = (decimal)0.5,
                TraderHDP = 8,
                OtherTradersHDP = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
            };

            var actual = generateImplicitMinorMatch.GenerateImplicitMinorMatchForExplicitMinorMatch(minorMatch).ToList();
            DeepAssert.Equal(expected, actual[0], "HashKey");

            Assert.False(string.IsNullOrEmpty(actual[0].HashKey));
        }

        [Fact]
        public void GenerateMinorMatch_MainMatchandMinorMatch_Return7Match()
        {
            var assignments = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    League = "A",
                    Home = "B",
                    Away = "C",
                    SessionId = "sesionId1234",
                    HashKey = "hashkey12345",
                    MatchStatus = "Live",
                    EventDate = new DateTime(2020, 08, 19),
                    KickOffTime = new TimeSpan(0, 15, 0, 0),

                    MaxBetXMins = 5000,
                    SpreadXMins = (decimal)0.5,
                    Trader15Mins = 8,
                    OtherTraders15Mins = "<Root><r u=\"0\" n=\"starix1\" /></Root>",
                    Trader10Mins = 8,
                    OtherTraders10Mins = "<Root><r u=\"0\" n=\"starix1\" /></Root>",
                    MaxBetFM = 5000,
                    SpreadFM = (decimal)0.5,
                    TraderFM = 8,
                    OtherTradersFM = "<Root><r u=\"0\" n=\"starix1\" /></Root>",
                    TraderAIO = 8,
                    OtherTradersAIO = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
                },
                new AssignmentModel
                {
                    MinorBettypeId = -6,
                    League = "A - BOOKING",
                    Home = "B Total Bookings",
                    Away = "C Total Bookings",
                    SessionId = "sesionId1234",
                    ParentHashKey = "hashkey12345",
                    HashKey = "childHash",
                    MatchStatus = "Live",
                    EventDate = new DateTime(2020, 08, 19),
                    KickOffTime = new TimeSpan(0, 15, 0, 0),

                    MaxBetXMins = 5000,
                    SpreadXMins = (decimal)0.5,
                    Trader15Mins = 8,
                    OtherTraders15Mins = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
                }
            };

            var expected = new List<AssignmentModel> { assignments[0] };
            expected.AddRange(new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    MinorBettypeId = -1,
                    ParentHashKey = "hashkey12345",
                    SessionId = "sesionId1234",

                    MatchStatus = "Live",
                    EventDate = new DateTime(2020, 08, 19),
                    KickOffTime = new TimeSpan(0, 15, 0, 0),
                    IsImplicitMinorMatch = true,

                    League = "A - SPECIFIC 15 MINS",
                    Home = "B (SPECIFIC 15 MINS)",
                    Away = "C (SPECIFIC 15 MINS)",

                    MaxBetHDP = 5000,
                    SpreadHDP = (decimal)0.5,
                    TraderHDP = 8,
                    OtherTradersHDP = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
                },
                new AssignmentModel
                {
                    MinorBettypeId = -2,
                    ParentHashKey = "hashkey12345",
                    SessionId = "sesionId1234",

                    MatchStatus = "Live",
                    EventDate = new DateTime(2020, 08, 19),
                    KickOffTime = new TimeSpan(0, 15, 0, 0),
                    IsImplicitMinorMatch = true,

                    League = "A - SPECIFIC 10 MINS",
                    Home = "B (SPECIFIC 10 MINS)",
                    Away = "C (SPECIFIC 10 MINS)",

                    MaxBetHDP = 5000,
                    SpreadHDP = (decimal)0.5,
                    TraderHDP = 8,
                    OtherTradersHDP = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
                },
                new AssignmentModel
                {
                    MinorBettypeId = -88,
                    ParentHashKey = "hashkey12345",
                    SessionId = "sesionId1234",

                    MatchStatus = "Live",
                    EventDate = new DateTime(2020, 08, 19),
                    KickOffTime = new TimeSpan(0, 15, 0, 0),
                    IsImplicitMinorMatch = true,

                    League = "A - FAST MARKET",
                    Home = "B Fast Market",
                    Away = "C Fast Market",

                    MaxBetHDP = 5000,
                    SpreadHDP = (decimal)0.5,
                    TraderHDP = 8,
                    OtherTradersHDP = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
                },
                new AssignmentModel
                {
                    MinorBettypeId = -89,
                    ParentHashKey = "hashkey12345",
                    SessionId = "sesionId1234",

                    MatchStatus = "Live",
                    EventDate = new DateTime(2020, 08, 19),
                    KickOffTime = new TimeSpan(0, 15, 0, 0),
                    IsImplicitMinorMatch = true,

                    League = "A - ALL IN ONE",
                    Home = "B All In One",
                    Away = "C All In One",

                    TraderHDP = 8,
                    OtherTradersHDP = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
                }
            });

            expected.Add(assignments[1]);
            expected.AddRange(new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    MinorBettypeId = -8,
                    ParentHashKey = "hashkey12345",
                    SessionId = "sesionId1234",

                    MatchStatus = "Live",
                    EventDate = new DateTime(2020, 08, 19),
                    KickOffTime = new TimeSpan(0, 15, 0, 0),
                    IsImplicitMinorMatch = true,

                    League = "A - SPECIFIC 15 MINS TOTAL BOOKINGS",
                    Home = "B Total Bookings (SPECIFIC 15 MINS)",
                    Away = "C Total Bookings (SPECIFIC 15 MINS)",
                    MaxBetHDP = 5000,
                    SpreadHDP = (decimal)0.5,
                    TraderHDP = 8,
                    OtherTradersHDP = "<Root><r u=\"0\" n=\"starix1\" /></Root>"
                }
            });

            var actual = generateImplicitMinorMatch.GenerateMinorMatch(assignments);
            DeepAssert.Equal(expected[0], actual[0], "HashKey");
            DeepAssert.Equal(expected[1], actual[1], "HashKey");
            DeepAssert.Equal(expected[2], actual[2], "HashKey");
            DeepAssert.Equal(expected[3], actual[3], "HashKey");
            DeepAssert.Equal(expected[4], actual[4], "HashKey");
            DeepAssert.Equal(expected[5], actual[5], "HashKey");
            DeepAssert.Equal(expected[6], actual[6], "HashKey");
            DeepAssert.Equal(expected, actual, "HashKey");

            Assert.False(string.IsNullOrEmpty(actual[0].HashKey));
            Assert.False(string.IsNullOrEmpty(actual[1].HashKey));
            Assert.False(string.IsNullOrEmpty(actual[2].HashKey));
            Assert.False(string.IsNullOrEmpty(actual[3].HashKey));
            Assert.False(string.IsNullOrEmpty(actual[4].HashKey));
            Assert.False(string.IsNullOrEmpty(actual[5].HashKey));
            Assert.False(string.IsNullOrEmpty(actual[6].HashKey));
        }
    }
}