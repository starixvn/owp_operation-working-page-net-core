namespace OWP.AssignmentUpload.UnitTests.Validation.Services
{
    using System;
    using System.Collections.Generic;
    using AssignmentUpload.Validation;
    using AssignmentUpload.Validation.Models;
    using AssignmentUpload.Validation.Services;
    using AutoMapper;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class AssignmentMapperImpUnitTests
    {
        private readonly AssignmentMapperImp assignmentMapper;

        public AssignmentMapperImpUnitTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AssignmentModelInExcel, AssignmentModel>();
            });
            var iMapper = config.CreateMapper();

            assignmentMapper = new AssignmentMapperImp(iMapper);

            var traders = new List<Trader>
            {
                new Trader{ UserId = 1, Username = "Mira", IsDuplicate = false },
                new Trader{ UserId = 3, Username = "Helen", IsDuplicate = false },
                new Trader{ UserId = 2, Username = "Lina", IsDuplicate = true }
            };
            assignmentMapper.Traders = traders;
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("Helen", 3)]
        [InlineData("HELEN", 3)]
        [InlineData("mira", 1)]
        public void GetTraderId_Always_ReturnsId(string name, int expected)
        {
            var actual = assignmentMapper.GetTraderId(name);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExtractNote_TextIsMatchRegex_ReturnIsUserText()
        {
            const string otherTraders = "#Mira";
            const string expected = " #<user> ";
            var actual = assignmentMapper.ExtractNote(otherTraders);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MapAssignment_FromValidationRequestAndAssignmentModelInExcel_ReturnModelAssignmentCorrect()
        {
            var excelModels = new List<AssignmentModelInExcel>
            {
                new AssignmentModelInExcel
                {
                   Agent = "Sori",
                Away = "TPHCM",
                Channel = "vtv1",
                IPTV = "vtv1",
                Division = "h1",
                Home = "HN",
                InfoSiteMatchId = 0,
                AdminSiteId = 0,
                League = "V-League",
                MatchNo = "2",
                MatchOffer = "draw",
                MatchStatus = "",
                MaxBet1H1X2 = 2,
                MaxBet1HHDP = 3,
                MaxBet1HOU = 1,
                MaxBet1X2 = 2,
                MaxBetFM = 1,
                MaxBetHDP = 1,
                MaxBetOU = 1,
                MaxBetXMins = 1,
                NoHDP = "a",
                OtherOfTime = "03/24/2020 08:15:22",
                OtherProvider = "u",
                OtherTraders10Mins = "a",
                OtherTraders15Mins = "a",
                OtherTraders1H1X2 = "#Mira",
                OtherTraders1HHDP = "v",
                OtherTraders1HOU = "u",
                OtherTraders1X2 = "i",
                OtherTradersAIO = "i",
                OtherTradersFM = "#Lina",
                OtherTradersHDP = "o",
                OtherTradersLA = "p",
                OtherTradersOU = "p",
                Phone = "0123456789",
                Provider = "ABV",
                Remark = "s",
                Round = "1",
                Spread1H1X2 = 1,
                Spread1HHDP = 3,
                Spread1HOU = 1,
                Spread1X2 = 1,
                SpreadFM = 1,
                SpreadHDP = 1,
                SpreadOU = 1,
                SpreadXMins = 1,
                TV = "vtv",
                Time = "08/18/2018 07:22:16",
                Trader10MinsName = "",
                Trader15MinsName = "Mira",
                Trader1H1X2Name = "",
                Trader1HHDPName = "",
                Trader1HOUName = "",
                Trader1X2Name = "",
                TraderAIOName = "Lina",
                TraderFMName = "",
                TraderHDPName = "",
                TraderLAName = "Helen",
                TraderOUName = "",
                WB = "watchbet",
                Website1 = "www.abc.com",
                Website2 = "www.abc1.com",
                Website3 = "www.abc2.com",
                Website4 = "www.abc3.com"
                }
            };

            var request = new ValidationRequest
            {
                EventDate = DateTime.Now
            };

            var result = assignmentMapper.Map(request, excelModels);
            Assert.Equal("V-League", result[0].League);
            Assert.Equal("HN", result[0].Home);
            Assert.Equal("TPHCM", result[0].Away);
        }

        [Fact]
        public void ExtractNote_MutipleTextIsMatchRegex_ReturnIsUserText()
        {
            const string otherTraders = "#Mira #Miraaa #Lina";
            const string expected = " #<user> #Miraaa #Lina";
            var actual = assignmentMapper.ExtractNote(otherTraders);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExtractNote_TextIsNotMatchRegex_ReturnInputText()
        {
            const string otherTraders = "Liii3o";
            const string expected = "Liii3o";
            var actual = assignmentMapper.ExtractNote(otherTraders);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExtractNote_EmptyText_ReturnEmpty()
        {
            const string otherTraders = "";
            const string expected = "";
            var actual = assignmentMapper.ExtractNote(otherTraders);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExtractTrader_EmptyText_Return0()
        {
            const string otherTraders = "";
            const int expected = 0;
            var actual = assignmentMapper.ExtractTrader(otherTraders);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExtractTrader_TextIsNotMatchRegex_ReturnInputText()
        {
            const string otherTraders = "Liii3o";
            const int expected = 0;
            var actual = assignmentMapper.ExtractTrader(otherTraders);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExtractTrader_TextIsMatchRegex_Return0()
        {
            const string otherTraders = "#Helen#Nini";
            const int expected = 3;
            var actual = assignmentMapper.ExtractTrader(otherTraders);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetOtherTradersInXml_TraderIsNotMatchRegex_TraderIdShouldBe0andNoteShouldBeInputText()
        {
            const string otherTraders = "Nikii";

            const string expected = "0,Nikii";
            var actual = assignmentMapper.ParseOtherTraders(otherTraders);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetOtherTradersInXml_EmptyTrader_ReturnEmpty()
        {
            const string otherTraders = "";

            const string expected = "";
            var actual = assignmentMapper.ParseOtherTraders(otherTraders);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetOtherTradersInXml_TraderIsMatchRegex_TraderIdShouldBeUserIdandNoteShouldBeUserXml()
        {
            const string otherTraders = "#Mira";

            const string expected = "1, #<user> ";
            var actual = assignmentMapper.ParseOtherTraders(otherTraders);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetOtherTradersInXml_MutipleTraderConcludeMatchAndNotMatchRegex_ReturnXmlTraders()
        {
            const string otherTraders = "#Helen,   #Mira, Winner";

            const string expected = "3, #<user> <o>1, #<user> <o>0,Winner";
            var actual = assignmentMapper.ParseOtherTraders(otherTraders);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MapEventDate_KickOffTimeIsAfternoon_EventDayShouldBeCurrentDay()
        {
            var dateTime = new DateTime(2020, 3, 20);
            var excelModel = new AssignmentModelInExcel
            {
                Time = "03/20/2020 15:00:00"
            };
            var model = new AssignmentModel();

            assignmentMapper.MapEventDate(dateTime, excelModel, model);
            var expectedEventDate = new DateTime(2020, 3, 20, 0, 0, 0);

            Assert.Equal(expectedEventDate, model.EventDate);
        }

        [Fact]
        public void MapEventDate_KickOffTimeIsMorning_EventDayShouldBeNextDay()
        {
            var dateTime = new DateTime(2020, 3, 20);
            var excelModel = new AssignmentModelInExcel
            {
                Time = "03/18/2020 07:22:16"
            };
            var model = new AssignmentModel();

            assignmentMapper.MapEventDate(dateTime, excelModel, model);
            var expectedEventDate = new DateTime(2020, 3, 21, 0, 0, 0);

            Assert.Equal(expectedEventDate, model.EventDate);
        }

        [Fact]
        public void MapEventDate_KickOffTimeIsInvalid_FailMapEventDate()
        {
            var dateTime = new DateTime(2020, 3, 20);
            var excelModel = new AssignmentModelInExcel
            {
                Time = "abc"
            };
            var model = new AssignmentModel();

            assignmentMapper.MapEventDate(dateTime, excelModel, model);

            Assert.Equal(new DateTime(), model.EventDate);
        }

        [Fact]
        public void MapTime_ValidTime_SuccessMapTime()
        {
            var excelModel = new AssignmentModelInExcel
            {
                Time = "08/18/2018 07:22:16",
                OtherOfTime = "03/24/2020 08:15:22"
            };
            var model = new AssignmentModel();

            assignmentMapper.MapTime(excelModel, model);
            var expectedKickOffTime = new TimeSpan(0, 7, 22, 16, 0);
            var expectedOtherTime = new TimeSpan(0, 8, 15, 22, 0);

            Assert.Equal(expectedKickOffTime, model.KickOffTime);
            Assert.Equal(expectedOtherTime, model.OtherTime);
        }

        [Fact]
        public void MapTime_InValidTime_FailMapTime()
        {
            var excelModel = new AssignmentModelInExcel
            {
                Time = "abc",
                OtherOfTime = ""
            };
            var model = new AssignmentModel();

            assignmentMapper.MapTime(excelModel, model);
            var expectedKickOffTime = new TimeSpan(0, 0, 0, 0, 0);

            Assert.Equal(expectedKickOffTime, model.KickOffTime);
            Assert.Null(model.OtherTime);
        }

        [Fact]
        public void MapTraders_ValidTraderName_SuccessMapTrader()
        {
            var excelModel = new AssignmentModelInExcel
            {
                Trader10MinsName = "Mira",
                Trader15MinsName = "Lina",
                Trader1H1X2Name = "",
                Trader1HHDPName = "Helen"
            };
            var model = new AssignmentModel();

            assignmentMapper.MapTraders(excelModel, model);

            Assert.Equal(1, model.Trader10Mins);
            Assert.Equal(2, model.Trader15Mins);
            Assert.Equal(0, model.Trader1H1X2);
            Assert.Equal(3, model.Trader1HHDP);
        }

        [Theory]
        [InlineData("", "Live")]
        [InlineData("Live", "Live")]
        [InlineData("in-play", "Live")]
        [InlineData("In-Play", "Live")]
        [InlineData("completed", "Live")]
        [InlineData("Completed", "Live")]
        [InlineData("IN-PLAY", "Live")]
        [InlineData("postponed", "Postponed")]
        [InlineData("SUSpended", "Suspended")]
        [InlineData("CANCEL", "Cancel")]
        public void GetMatchStatus(string status, string expected)
        {
            var actual = assignmentMapper.GetMatchStatus(status);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MapStatus_StatusIsEmpty_MapIsLive()
        {
            var excelModel = new AssignmentModelInExcel
            {
                MatchStatus = ""
            };
            var model = new AssignmentModel();

            assignmentMapper.MapMatchStatus(excelModel, model);

            Assert.Equal("Live", model.MatchStatus);
        }

        [Fact]
        public void MapStatus_StatusIsNotEmpty_MapIsThisStatus()
        {
            var excelModel = new AssignmentModelInExcel
            {
                MatchStatus = "Cancel"
            };
            var model = new AssignmentModel();

            assignmentMapper.MapMatchStatus(excelModel, model);

            Assert.Equal("Cancel", model.MatchStatus);
        }

        [Fact]
        public void Map_ValidModel_ReturnsUserModel()
        {
            var excelModel = new AssignmentModelInExcel
            {
                Agent = "Sori",
                Away = "a",
                Channel = "vtv1",
                IPTV = "vtv1",
                Division = "h1",
                Home = "was",
                InfoSiteMatchId = 0,
                AdminSiteId = 0,
                League = "a",
                MatchNo = "2",
                MatchOffer = "draw",
                MatchStatus = "",
                MaxBet1H1X2 = 2,
                MaxBet1HHDP = 3,
                MaxBet1HOU = 1,
                MaxBet1X2 = 2,
                MaxBetFM = 1,
                MaxBetHDP = 1,
                MaxBetOU = 1,
                MaxBetXMins = 1,
                NoHDP = "a",
                OtherOfTime = "03/24/2020 08:15:22",
                OtherProvider = "u",
                OtherTraders10Mins = "a",
                OtherTraders15Mins = "a",
                OtherTraders1H1X2 = "#Mira",
                OtherTraders1HHDP = "v",
                OtherTraders1HOU = "u",
                OtherTraders1X2 = "i",
                OtherTradersAIO = "i",
                OtherTradersFM = "#Lina",
                OtherTradersHDP = "o",
                OtherTradersLA = "p",
                OtherTradersOU = "p",
                Phone = "0123456789",
                Provider = "ABV",
                Remark = "s",
                Round = "1",
                Spread1H1X2 = 1,
                Spread1HHDP = 3,
                Spread1HOU = 1,
                Spread1X2 = 1,
                SpreadFM = 1,
                SpreadHDP = 1,
                SpreadOU = 1,
                SpreadXMins = 1,
                TV = "vtv",
                Time = "08/18/2018 07:22:16",
                Trader10MinsName = "",
                Trader15MinsName = "Mira",
                Trader1H1X2Name = "",
                Trader1HHDPName = "",
                Trader1HOUName = "",
                Trader1X2Name = "",
                TraderAIOName = "Lina",
                TraderFMName = "",
                TraderHDPName = "",
                TraderLAName = "Helen",
                TraderOUName = "",
                WB = "watchbet",
                Website1 = "www.abc.com",
                Website2 = "www.abc1.com",
                Website3 = "www.abc2.com",
                Website4 = "www.abc3.com"
            };
            var expected = new AssignmentModel
            {
                Agent = "Sori",
                Away = "a",
                Channel = "vtv1",
                IPTV = "vtv1",
                Division = "h1",
                EventDate = new DateTime(2020, 3, 16, 0, 0, 0),
                Home = "was",
                InfoSiteMatchId = 0,
                AdminSiteId = 0,
                KickOffTime = new TimeSpan(0, 7, 22, 16, 0),
                League = "a",
                MatchNo = "2",
                MatchOffer = "draw",
                MatchStatus = "Live",
                MaxBet1H1X2 = 2,
                MaxBet1HHDP = 3,
                MaxBet1HOU = 1,
                MaxBet1X2 = 2,
                MaxBetFM = 1,
                MaxBetHDP = 1,
                MaxBetOU = 1,
                MaxBetXMins = 1,
                NoHDP = "a",

                OtherProvider = "u",
                OtherTime = new TimeSpan(0, 8, 15, 22, 0),
                OtherTraders10Mins = "0,a",
                OtherTraders15Mins = "0,a",
                OtherTraders1H1X2 = "1, #<user> ",
                OtherTraders1HHDP = "0,v",
                OtherTraders1HOU = "0,u",
                OtherTraders1X2 = "0,i",
                OtherTradersAIO = "0,i",
                OtherTradersFM = "2, #<user> ",
                OtherTradersHDP = "0,o",
                OtherTradersLA = "0,p",
                OtherTradersOU = "0,p",
                Phone = "0123456789",
                Provider = "ABV",
                Remark = "s",
                Round = "1",
                SessionId = string.Empty,
                Spread1H1X2 = 1,
                Spread1HHDP = 3,
                Spread1HOU = 1,
                Spread1X2 = 1,
                SpreadFM = 1,
                SpreadHDP = 1,
                SpreadOU = 1,
                SpreadXMins = 1,
                TV = "vtv",
                Trader10Mins = 0,
                Trader15Mins = 1,
                Trader1H1X2 = 0,
                Trader1HHDP = 0,
                Trader1HOU = 0,
                Trader1X2 = 0,
                TraderAIO = 2,
                TraderFM = 0,
                TraderHDP = 0,
                TraderLA = 3,
                TraderOU = 0,
                WB = "watchbet",
                Website1 = "www.abc.com",
                Website2 = "www.abc1.com",
                Website3 = "www.abc2.com",
                Website4 = "www.abc3.com"
            };
            var dateTime = new DateTime(2020, 3, 15);
            var request = new ValidationRequest
            {
                IsUploadFMAndAIO = true,
                EventDate = dateTime
            };
            var actual = assignmentMapper.Map(request, excelModel);

            DeepAssert.Equal(expected, actual, "HashKey", "SessionId");
            Assert.False(string.IsNullOrEmpty(actual.HashKey));
        }

        [Theory]
        [InlineData("A(N)", "A")]
        [InlineData("A (N)", "A")]
        [InlineData("A(N) ", "A")]
        [InlineData("A (N) ", "A")]
        [InlineData("A(n) ", "A")]
        [InlineData("A(n)B ", "A B")]
        [InlineData("A (N) B ", "A B")]
        [InlineData("OC (N) Next Corners", "OC Next Corners")]
        public void RemoveNeutralSign_AlwaysRemoveIngnoreCase(string input, string expected)
        {
            var actual = AssignmentMapperImp.RemoveNeutralSign(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MapIsNeutral_Correct()
        {
            var model = new AssignmentModel();
            var excelModels = new AssignmentModelInExcel
            {
                MatchNo = "123",
                League = "Seagame",
                Home = "VN(N)",
                Away = "Thai lan",
                Time = "2020-10-29",
            };

            var expected = new AssignmentModel
            {
                Home = "VN",
                IsNeutral = true
            };

            AssignmentMapperImp.MapIsNeutral(excelModels, model);

            DeepAssert.Equal(expected, model);
        }
    }
}