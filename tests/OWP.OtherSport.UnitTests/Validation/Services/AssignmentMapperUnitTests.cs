namespace OWP.OtherSport.UnitTests.Validation.Services;

using AutoMapper;
using OtherSport.Validation.Models;
using OtherSport.Validation.Services;
using Xunit;
using Xunit.Asserts.Compare;

public class AssignmentMapperUnitTests
{
    private readonly AssignmentMapperImp assignmentMapper;

    public AssignmentMapperUnitTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AssignmentModelInExcel, AssignmentModel>()
                .ForMember(dest => dest.DateTime, opt => opt.Ignore());
        });
        var iMapper = config.CreateMapper();

        assignmentMapper = new AssignmentMapperImp(iMapper);

        var traders = new List<Trader>
        {
            new() { UserId = 1, Username = "Esther"},
            new() { UserId = 10, Username = "Mira"},
            new() { UserId = 2, Username = "Lita"}
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
    public void ExtractNote_TextIsMatchRegex_ReturnText()
    {
        const string otherTraders = "ahihi #esther";
        const string expected = "ahihi #<user> ";

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
            TraderBadmintonAllProductPointWinner = "#Mira",
            TraderBadmintonMatchHandicap = "#Esther",
            TraderBasketball1HHDP = "",
            TraderBaseballFTTotalRunOU = "Lita"
        };

        var model = new AssignmentModel();

        assignmentMapper.MapTradersWithNote(excelModel, model);

        Assert.Equal("10, #<user> ", model.TraderBadmintonAllProductPointWinner);
        Assert.Equal("1, #<user> ", model.TraderBadmintonMatchHandicap);
        Assert.Equal("", model.TraderBasketball1HHDP);
        Assert.Equal("0,Lita", model.TraderBaseballFTTotalRunOU);
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

        assignmentMapper.MapTradersWithNote(excelModel, model);

        Assert.Equal("10, #<user> ", model.OtherTraderBadmintonAllProductPointWinner);
        Assert.Equal("1, #<user> ", model.OtherTraderBadmintonNextPointWinner);
        Assert.Equal("", model.OtherTraderBaseballFTRunLineHDP);
        Assert.Equal("0,Lita", model.OtherTraderBasketball1HHDP);
    }

    [Fact]
    public void MapMaxbets_ValidMaxbet_SuccessMapMaxbets()
    {
        var excelModel = new AssignmentModelInExcel
        {
            MaxBetBadmintonAllProductPointWinner = "1234.0125",
            MaxBetBaseball1H1st5inningsOU = "1245.1234"
        };

        var model = new AssignmentModel();

        assignmentMapper.MapMaxbetsAndSpreads(excelModel, model);

        Assert.Equal(1234.0125m, model.MaxBetBadmintonAllProductPointWinner);
        Assert.Equal(1245.1234m, model.MaxBetBaseball1H1st5inningsOU);
    }

    [Fact]
    public void MapIsNeutralMatch_ReturnNewHomeName()
    {
        var excelModel = new AssignmentModelInExcel
        {
            Home = "Team A(N)"
        };

        var model = new AssignmentModel();

        assignmentMapper.MapIsNeutralMatch(excelModel, model);

        Assert.Equal("Team A", model.Home);
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
            AutoLink = "Yes",
            Away = "LA",
            Home = "GSW",
            DateTime = "44175",
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
            Reporter = null,
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