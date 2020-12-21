namespace OWP.OtherSport.UnitTests.Validation.Services
{
    using AutoMapper;
    using OWP.OtherSport.Validation.Models;
    using OWP.OtherSport.Validation.Services;
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class AssignmentMapperUnitTests
    {
        private readonly AssignmentMapperImp assignmentMapper;
        public AssignmentMapperUnitTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AssignmentModel, AssignmentModelInExcel>();
            });
            var iMapper = config.CreateMapper();

            assignmentMapper = new AssignmentMapperImp(iMapper);

            var traders = new List<Trader>
            {
                new Trader{ UserId = 1, Username = "Esther"},
                new Trader{ UserId = 10, Username = "Mira"},
                new Trader{ UserId = 2, Username = "Lita"}
            };
            assignmentMapper.Traders = traders;
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("Esther", 1)]
        [InlineData("esther", 1)]
        [InlineData("mira", 10)]
        public void GetTraderId_Always_ReturnsId(string name, int expected)
        {
            var actual = assignmentMapper.GetTraderId(name);
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
            const string otherTraders = "keanu";
            const int expected = 0;
            var actual = assignmentMapper.ExtractTrader(otherTraders);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExtractTrader_TextIsMatchRegex_Return0()
        {
            const string otherTraders = "#Mira#lita";
            const int expected = 10;
            var actual = assignmentMapper.ExtractTrader(otherTraders);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExtractNote_TextIsMatchRegex_ReturnIsUserText()
        {
            const string otherTraders = "#esther";
            const string expected = " #<user> ";
            var actual = assignmentMapper.ExtractNote(otherTraders);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExtractNote_MutipleTextIsMatchRegex_ReturnIsUserText()
        {
            const string otherTraders = "#esther #halo #mira";
            const string expected = " #<user> #halo #mira";
            var actual = assignmentMapper.ExtractNote(otherTraders);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExtractNote_TextIsNotMatchRegex_ReturnInputText()
        {
            const string otherTraders = "noted";
            const string expected = "noted";
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
        public void GetOtherTradersInXml_TraderIsNotMatchRegex_TraderIdShouldBe0andNoteShouldBeInputText()
        {
            const string otherTrader = "Nikii";

            const string expected = "0,Nikii";
            var actual = assignmentMapper.ParseOtherTrader(otherTrader);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetOtherTradersInXml_EmptyTrader_ReturnEmpty()
        {
            const string otherTrader = "";

            const string expected = "";
            var actual = assignmentMapper.ParseOtherTrader(otherTrader);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetOtherTradersInXml_TraderIsMatchRegex_TraderIdShouldBeUserIdandNoteShouldBeUserXml()
        {
            const string otherTrader = "#esther";

            const string expected = "1, #<user> ";
            var actual = assignmentMapper.ParseOtherTrader(otherTrader);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MapTraders_ValidTraderName_SuccessMapTrader()
        {
            var excelModel = new AssignmentModelInExcel
            {
                TraderBadmintonAllProductPointWinner = "Mira",
                TraderBadmintonMatchHandicap = "Esther",
                TraderBasketball1HHDP = "",
                TraderBaseballFTTotalRunOU = "Lita"
            };
            var model = new AssignmentModel();

            assignmentMapper.MapTraders(excelModel, model);

            Assert.Equal(10, model.TraderBadmintonAllProductPointWinner);
            Assert.Equal(1, model.TraderBadmintonMatchHandicap);
            Assert.Equal(0, model.TraderBasketball1HHDP);
            Assert.Equal(2, model.TraderBaseballFTTotalRunOU);
        }

        [Fact]
        public void MapOtherTraders_ValidTraderName_SuccessMapTrader()
        {
            var excelModel = new AssignmentModelInExcel
            {
                OtherTraderBadmintonAllProductPointWinner = "#Mira",
                OtherTraderBadmintonNextPointWinner = "#Esther",
                OtherTraderBaseballFTRunLineHDP = "",
                OtherTraderBasketball1HHDP = "#Lita"
            };
            var model = new AssignmentModel();

            assignmentMapper.MapOtherTraders(excelModel, model);

            Assert.Equal("10, #<user> ", model.OtherTraderBadmintonAllProductPointWinner);
            Assert.Equal("1, #<user> ", model.OtherTraderBadmintonNextPointWinner);
            Assert.Equal("", model.OtherTraderBaseballFTRunLineHDP);
            Assert.Equal("2, #<user> ", model.OtherTraderBasketball1HHDP);
        }

        [Fact]
        public void Map_ValidModel_ReturnsUserModel()
        {
            var excelModel = GetAssignmentModelInExcel();
            var expected = ExpectedAssignment();

            var actual = assignmentMapper.Map(excelModel);

            DeepAssert.Equal(expected, actual);
        }

        private static AssignmentModelInExcel GetAssignmentModelInExcel()
        {
            var excelModel = new AssignmentModelInExcel
            {               
                AutoLink = "auto link",
                Away = "LA Laker",
                Home = "Golden State Warrior",
                Sport = "Basketball",
                TraderBadmintonAllProductPointWinner = "esther",
                TraderBadmintonAllProductSetWinner = "mira",
                TraderBadmintonMatchHandicap = "",
                TraderBadmintonNextPointWinner = "",
                TraderBadmintonRacetoPoints7911 = "",
                TraderBaseball1H1st5InningsHDP = "",
                TraderBaseball1H1st5InningsOU = "",
                TraderBaseballFTRunLineHDP = "",
                TraderBaseballFTTotalRunOU = "",
                TraderBaseballInningHitsRuns = "",
                TraderBasketball1HHDP = "",
                TraderBasketball1HOUOE = "",
                TraderBasketball1Q3QHDP = "",
                TraderBasketball1Q3QOUOE = "",
                TraderBasketballFTHDP = "",
                TraderBasketballFTOUOE = "",
                TraderCricketFT1X2 = "",
                TraderCricketMatchWinnerInningsOU = "",
                TraderCricketNextOverOUPlayerOU = "",
                TraderDartsLegsHDP = "",
                OtherTraderBadmintonAllProductPointWinner = "#esther",
                OtherTraderBadmintonAllProductSetWinner = "",
                DateTime = "Dec, 12, 2021",
                League = "NBA",
                MatchStatus = "Live",
                NoHDP = "5s",
                MaxBetBadmintonAllProductSetWinner = 3,
                MaxBetBadmintonNextPointWinner = 5,
                MaxBetBadmintonAllProductPointWinner = 1,
                MaxBetBadmintonMatchHandicap = 1,
                MaxBetBadmintonRacetoPoints7911 = 1,
                MaxBetBaseball1H1st5inningsHDP = 2,
                MaxBetBaseball1H1st5inningsOU = 3,
                MaxBetBaseballFTRunLineHDP = 4, 
                MaxBetBaseballFTTotalRunOU = 3,
                MaxBetBaseballInningHitsRuns = 3,
                MaxBetBasketballFTHDP = 2,
                MaxBetBasketballFTOE = 1,
                MaxBetCricketFT1X2 = 1,
                MaxBetCricketMatchWinnerInningsOU = 2,
                MaxBetCricketNextOverOUPlayerOU = 1,
                MaxBetDartsLegsHDP = 3,
                MaxBetDartsMost180sHDP = 1,
                MaxBetFieldHockey1HHDP = 1,
                MaxBetFieldHockey1HOU = 2,
                MaxBetFieldHockey1QHDP = 2,
                MaxBetFieldHockey1QOU = 1,
                MaxBetFieldHockeyFTHDP = 1,
                MaxBetFieldHockeyFTOU = 7,
                MaxBetFootball1HHDP = 7,
                MaxBetFootball1HOU = 5,
                MaxBetFootball1Q3QHDP = 7,
                MaxBetFootball1Q3QOU = 4,
                MaxBetFootballFTHDP = 4,
                MaxBetFootballFTOU = 1,
                MaxBetHandball1HHDP = 2,
                MaxBetHandball1HOU = 1,
                MaxBetHandballFTHDP = 1,
                MaxBetHandballFTOU = 1,
                MaxBetIceHockey1st2ndPeriodsHDP = 3,
                MaxBetIceHockey1st2ndPeriodsOU = 5,
                MaxBetIceHockeyFTHDP = 4,
                MaxBetNetball1HHDP = 2, 
                MaxBetIceHockeyFTOU = 1,
                MaxBetNetball1HOUOE = 3,
                MaxBetNetball1Q3QHDP = 3,
                MaxBetNetball1Q3QOUOE = 1,
                MaxBetNetballFTHDP = 3,
                MaxBetNetballFTOUOE = 4
            };

            return excelModel;
        }

        private static AssignmentModel ExpectedAssignment()
        {
            var excelModel = new AssignmentModel
            {
                AutoLink = "auto link",
                Away = "LA Laker",
                Home = "Golden State Warrior",
                Sport = "Basketball",
                TraderBadmintonAllProductPointWinner = 1,
                TraderBadmintonAllProductSetWinner = 10,
                TraderBadmintonMatchHandicap = 0,
                TraderBadmintonNextPointWinner = 0,
                TraderBadmintonRacetoPoints7911 = 0,
                TraderBaseball1H1st5InningsHDP = 0,
                TraderBaseball1H1st5InningsOU = 0,
                TraderBaseballFTRunLineHDP = 0,
                TraderBaseballFTTotalRunOU = 0,
                TraderBaseballInningHitsRuns = 0,
                TraderBasketball1HHDP = 0,
                TraderBasketball1HOUOE = 0,
                TraderBasketball1Q3QHDP = 0,
                TraderBasketball1Q3QOUOE = 0,
                TraderBasketballFTHDP = 0,
                TraderBasketballFTOUOE = 0,
                TraderCricketFT1X2 = 0,
                TraderCricketMatchWinnerInningsOU = 0,
                TraderCricketNextOverOUPlayerOU = 0,
                TraderDartsLegsHDP = 0,
                OtherTraderBadmintonAllProductPointWinner = "1, #<user> ",
                OtherTraderBadmintonAllProductSetWinner = "",
                DateTime = DateTime.Parse("Dec, 12, 2021"),
                League = "NBA",
                MatchStatus = "Live",
                NoHDP = "5s",
                MaxBetBadmintonAllProductSetWinner = 3,
                MaxBetBadmintonNextPointWinner = 5,
                MaxBetBadmintonAllProductPointWinner = 1,
                MaxBetBadmintonMatchHandicap = 1,
                MaxBetBadmintonRacetoPoints7911 = 1,
                MaxBetBaseball1H1st5inningsHDP = 2,
                MaxBetBaseball1H1st5inningsOU = 3,
                MaxBetBaseballFTRunLineHDP = 4,
                MaxBetBaseballFTTotalRunOU = 3,
                MaxBetBaseballInningHitsRuns = 3,
                MaxBetBasketballFTHDP = 2,
                MaxBetBasketballFTOE = 1,
                MaxBetCricketFT1X2 = 1,
                MaxBetCricketMatchWinnerInningsOU = 2,
                MaxBetCricketNextOverOUPlayerOU = 1,
                MaxBetDartsLegsHDP = 3,
                MaxBetDartsMost180sHDP = 1,
                MaxBetFieldHockey1HHDP = 1,
                MaxBetFieldHockey1HOU = 2,
                MaxBetFieldHockey1QHDP = 2,
                MaxBetFieldHockey1QOU = 1,
                MaxBetFieldHockeyFTHDP = 1,
                MaxBetFieldHockeyFTOU = 7,
                MaxBetFootball1HHDP = 7,
                MaxBetFootball1HOU = 5,
                MaxBetFootball1Q3QHDP = 7,
                MaxBetFootball1Q3QOU = 4,
                MaxBetFootballFTHDP = 4,
                MaxBetFootballFTOU = 1,
                MaxBetHandball1HHDP = 2,
                MaxBetHandball1HOU = 1,
                MaxBetHandballFTHDP = 1,
                MaxBetHandballFTOU = 1,
                MaxBetIceHockey1st2ndPeriodsHDP = 3,
                MaxBetIceHockey1st2ndPeriodsOU = 5,
                MaxBetIceHockeyFTHDP = 4,
                MaxBetNetball1HHDP = 2,
                MaxBetIceHockeyFTOU = 1,
                MaxBetNetball1HOUOE = 3,
                MaxBetNetball1Q3QHDP = 3,
                MaxBetNetball1Q3QOUOE = 1,
                MaxBetNetballFTHDP = 3,
                MaxBetNetballFTOUOE = 4,
                MaxBetOtherSportsMatchWinner = 3
            };

            return excelModel;
        }
    }
}
