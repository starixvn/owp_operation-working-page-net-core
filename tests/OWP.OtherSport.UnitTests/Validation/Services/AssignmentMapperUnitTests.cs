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
                cfg.CreateMap<AssignmentModelInExcel, AssignmentModel>();
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
                OtherTraderBasketball1HHDP = "Lita"
            };
            var model = new AssignmentModel();

            assignmentMapper.MapOtherTraders(excelModel, model);

            Assert.Equal("10, #<user> ", model.OtherTraderBadmintonAllProductPointWinner);
            Assert.Equal("1, #<user> ", model.OtherTraderBadmintonNextPointWinner);
            Assert.Equal("", model.OtherTraderBaseballFTRunLineHDP);
            Assert.Equal("0,Lita", model.OtherTraderBasketball1HHDP);
        }

        [Fact]
        public void Map_ValidModel_ReturnsUserModel()
        {
            var excelModel = GetAssignmentModelInExcel();
            var expected = ExpectedAssignment();

            var actual = assignmentMapper.Map(excelModel);

            DeepAssert.Equal(expected, actual, "HashKey");
            Assert.False(string.IsNullOrEmpty(actual.HashKey));
        }

        private static AssignmentModelInExcel GetAssignmentModelInExcel()
        {
            var excelModel = new AssignmentModelInExcel
            {
                AutoLink = "Yes",
                Away = "LA",
                Home = "GSW",
                DateTime = "12/10/2020 12:00 AM",
                Sport = "BasketBall",                             
                League = "NBA",
                MatchStatus = "Live"    
            };

            return excelModel;
        }

        private static AssignmentModel ExpectedAssignment()
        {
            var model = new AssignmentModel
            {
                AutoLink = "Yes",
                Away = "LA",
                Home = "GSW",
                DateTime = DateTime.Parse("12/10/2020 12:00 AM"),
                Sport = "BasketBall",
                League = "NBA",
                MatchStatus = "Live"
            };

            return model;
        }
    }
}
