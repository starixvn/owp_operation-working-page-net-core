using Newtonsoft.Json;
using NSubstitute;
using OWP.NewLeagueSetting.Data;
using OWP.NewLeagueSetting.Models;
using OWP.NewLeagueSetting.Service;
using Xunit;

public class DuplicateSettingServiceUnitTest
{
    private readonly Repository _repository;
    private readonly DuplicateSettingServiceImp _service;

    public DuplicateSettingServiceUnitTest()
    {
        _repository = Substitute.For<Repository>();
        _service = new DuplicateSettingServiceImp(_repository);
    }

    [Fact]
    public async Task GetDuplicatedSettingId_ReturnsTrue_WhenDuplicateExists()
    {
        // Arrange
        var model = new LeagueSettingRequest
        {
            League = "Premier League",
            LeagueSettingId = 1,
            HasIpTv = true,
            HasTv = true,
            Round = new List<Condition> { new Condition("Round", "1") },
            Division = new List<Condition> { new Condition("Division", "1") }
        };

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            new ()
            {
                LeagueSettingId = 2,
                FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition
                {
                    HasIpTv = true,
                    HasTv = true,
                    Round = new List<Condition> { new Condition("Round", "1") },
                    Division = new List<Condition> { new Condition("Division", "1") }
                })
            }
        };

        _repository.QueryLeagueSetting(model.League, true).Returns(leagueSettings);

        // Act
        var result = await _service.GetDuplicatedSettingId(model);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetDuplicatedSettingId_ReturnsFalse_WhenNoDuplicateExists()
    {
        // Arrange
        var model = new LeagueSettingRequest
        {
            League = "Premier League",
            LeagueSettingId = 1,
            HasIpTv = true,
            HasTv = true,
            Round = new List<Condition> { new Condition("Round", "1") },
            Division = new List<Condition> { new Condition("Division", "1") }
        };

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            new LeagueSettingInfoSender
            {
                LeagueSettingId = 2,
                FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition
                {
                    HasIpTv = false,
                    HasTv = true,
                    Round = new List<Condition> { new Condition("Round", "2") },
                    Division = new List<Condition> { new Condition("Division", "2") }
                })
            }
        };

        _repository.QueryLeagueSetting(model.League, false).Returns(leagueSettings);

        // Act
        var result = await _service.GetDuplicatedSettingId(model);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetDuplicatedSettingId_ReturnsTrue_WhenCaseInsensitiveMatch()
    {
        // Arrange
        var model = new LeagueSettingRequest
        {
            League = "Premier League",
            LeagueSettingId = 1,
            HasIpTv = true,
            HasTv = true,
            Round = new List<Condition> { new Condition("Round", "1") },
            Division = new List<Condition> { new Condition("Division", "1") }
        };

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            new LeagueSettingInfoSender
            {
                LeagueSettingId = 2,
                FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition
                {
                    HasIpTv = true,
                    HasTv = true,
                    Round = new List<Condition> { new Condition("round", "1") },
                    Division = new List<Condition> { new Condition("division", "1") }
                })
            }
        };

        _repository.QueryLeagueSetting(model.League, true).Returns(leagueSettings);

        // Act
        var result = await _service.GetDuplicatedSettingId(model);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_ReturnsTrue_WhenCaseInsensitiveMatch()
    {
        // Arrange
        var condition1 = new Condition("Type", "Value");
        var condition2 = new Condition("type", "Value");
        var comparer = new ConditionComparer();

        // Act
        var result = comparer.Equals(condition1, condition2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsConditionsEqual_ReturnsTrue_WhenConditionsMatchIgnoringCase()
    {
        // Arrange
        var firstList = new List<Condition>
        {
            new Condition("Type1", "Value1"),
            new Condition("Type2", "Value2")
        };

        var secondList = new List<Condition>
        {
            new Condition("type1", "value1"),
            new Condition("TYPE2", "VALUE2")
        };

        // Act
        var result = DuplicateSettingServiceImp.IsConditionsEqual(firstList, secondList);

        // Assert
        Assert.True(result);
    }
}
