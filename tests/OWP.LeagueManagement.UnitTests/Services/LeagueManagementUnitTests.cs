using Fanex.NAP.Client.Helpers.Web;
using Newtonsoft.Json;
using NSubstitute;
using Olympus.Xml;
using OWP.LeagueManagement.Criteria;
using OWP.LeagueManagement.Models;
using OWP.LeagueManagement.Services;
using OWP.Models;
using Xunit;

namespace OWP.LeagueManagement.UnitTests.Services;

using Xunit.Asserts.Compare;

public class LeagueManagementUnitTests
{
    private readonly Repository repository;
    private readonly LeagueManagementService service;

    public LeagueManagementUnitTests()
    {
        repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        service = new LeagueManagementServiceImp(session, repository);
    }
    [Fact]
    public async Task UpdateTvList_Always_ReturnData()
    {
        // Arrange
        const int code = 0;
        var message = string.Empty;
        var model = new List<TVList> { new() { Label = "Contains", Value = "TL" } };
        repository.UpdateTVList(Arg.Compat.Any<UpdateTvListCriteria>()).Returns(Task.FromResult(new ResultDataObject { Code = code, Message = message }));

        // Act
        var actual = await service.UpdateTvList(model);

        // Assert
        Assert.Equal(code, actual.Code);
        Assert.Equal(message, actual.Message);
        await repository.Received().UpdateTVList(Arg.Compat.Is<UpdateTvListCriteria>(x => x.UpdateTvListXml == XmlConverter.Serialize(model)));
    }
    [Fact]
    public async Task UpdateCondition_Always_ReturnData()
    {
        // Arrange
        const int code = 0;
        var message = string.Empty;
        var model = new LeagueSettingCondition
        {
            HasTV = true,
            TV = string.Empty,
            LeagueSettingId = 1,
            Rounds = new List<Condition> { new() { Type = "Contains", Value = string.Empty } },
            Divisions = new List<Condition> { new() { Type = "Equals", Value = string.Empty } }
        };
        repository.UpdateCondition(Arg.Compat.Any<UpdateConditionCriteria>()).Returns(Task.FromResult(new ResultDataObject { Code = code, Message = message }));

        // Act
        var actual = await service.UpdateCondition(model);

        // Assert
        Assert.Equal(code, actual.Code);
        Assert.Equal(message, actual.Message);
        await repository.Received().UpdateCondition(Arg.Compat.Is<UpdateConditionCriteria>(x =>
            x.HasTV == model.HasTV &&
            x.TV == model.TV &&
            x.Round == string.Join("/", model.Rounds.Select(y => y.Type == "Contains" ? $"*{y.Value}*" : y.Value)) &&
            x.Division == string.Join("/", model.Divisions.Select(y => y.Type == "Contains" ? $"*{y.Value}*" : y.Value)) &&
            x.FullConditionDescription == JsonConvert.SerializeObject(model)
        ));
    }

    [Fact]
    public async Task GetCondition_Always_ReturnData()
    {
        const string conditionDesc = "{\"LeagueSettingId\":126,\"HasTV\":true,\"TV\":null,\"Rounds\":[{\"Type\":\"Equals\",\"Value\":\"M2\"}],\"Divisions\":[{\"Type\":\"Equals\",\"Value\":\"Carlos\"}]}";
        var condition = new LeagueSettingCondition
        {
            Divisions = new List<Condition> { new() { Type = "Equals", Value = "Carlos" } },
            Rounds = new List<Condition> { new() { Type = "Equals", Value = "M2" } },
            HasTV = true,
            LeagueSettingId = 126,
            TV = null
        };
        repository.GetCondition(Arg.Compat.Any<GetConditionCriteria>()).Returns(Task.FromResult(conditionDesc));

        var actual = await service.GetCondition(126);

        DeepAssert.Equal(condition, actual);
    }
    [Theory]
    [InlineData("")]
    [InlineData("{'LeagueSettingId':1,'HasTV':false,'TV':'','Rounds':[],'Divisions':[]}")]
    public void ConvertToLeagueSettingCondition_Always_ReturnLeagueSettingCondition(string condition)
    {
        // Arrange
        var expected = new LeagueSettingCondition
        {
            LeagueSettingId = 1,
            HasTV = false,
            TV = string.Empty
        };

        // Act
        var actual = service.ConvertToLeagueSettingCondition(1, condition);

        // Assert
        DeepAssert.Equal(expected, actual);
    }
    [Fact]
    public void GetConditionRegex_ContainsType_Always_ReturnRegexWithStarCharacter()
    {
        // Arrange
        var condition = new Condition
        {
            Type = "Contains",
            Value = "AA"
        };

        // Act
        var actual = service.GetConditionRegex(condition);

        // Assert
        DeepAssert.Equal("*AA*", actual);
    }
    [Fact]
    public void GetConditionRegex_EqualType_Always_ReturnConditionValue()
    {
        // Arrange
        var condition = new Condition
        {
            Type = "Equals",
            Value = "AA"
        };

        // Act
        var actual = service.GetConditionRegex(condition);

        // Assert
        DeepAssert.Equal("AA", actual);
    }
    [Fact]
    public async Task GetAllLeague_Always_ReturnData()
    {
        var mockLeagueLinkToTeam = new List<int> { 1257 };
        var query = new LeagueInfoQuery();
        repository.GetAllLeague(Arg.Compat.Any<LeagueInfoQuery>()).Returns(MockLeagueInfoResponse());
        repository.GetLeagueLinkToTeam(Arg.Compat.Any<string>()).Returns(mockLeagueLinkToTeam);
        var actual = await service.GetAllLeague(query);

        DeepAssert.Equal(MockLeagueInfoResponse(), actual);
    }

    public List<LeagueInfoResponse> MockLeagueInfoResponse()
    {
        return new List<LeagueInfoResponse>
        {
            new()
            {
                CN88MaxFT1X2 = null,
                CN88MaxMaxbetWD = null,
                CN88MaxMaxbetWK = null,
                CN88MaxSpread = null,
                CN88MinFT1X2 = null,
                CN88MinMaxbetWD = null,
                CN88MinMaxbetWK = null,
                CN88MinSpread = null,
                GroupLabel = null,
                GroupValue = 0,
                HasLinkLeagueAndTeam = true,
                IsActive = true,
                IsBlackList = false,
                LeagueName = "*UEFA EUROPA LEAGUE - CORNERS",
                MaxFT1X2 = null,
                MaxMaxbetWD = null,
                MaxMaxbetWK = null,
                MaxSpread = null,
                MinFT1X2 = null,
                MinMaxbetWD = null,
                MinMaxbetWK = null,
                MinSpread = null,
                OWPLeagueId = 1257
            }
        };
    }
}