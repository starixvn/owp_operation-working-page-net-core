using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NSubstitute;
using OWP.NewLeagueSetting.Service;
using OWP.NewLeagueSetting.Models;
using OWP.NewLeagueSetting.Data;
using Xunit;

namespace OWP.NewLeagueSetting.UnitTests.Service;

public class DuplicateSettingServiceUnitTest
{
    [Fact]
    public async Task GetDuplicatedSettingId_WithNoDuplicates_ShouldReturnFalse()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var service = new DuplicateSettingServiceImp(repository);

        var model = new LeagueSettingRequest
        {
            LeagueSettingId = 1,
            League = "Test League",
            HasIpTv = true,
            HasTv = false,
            Round = new List<Condition> { new Condition("Round", "1") },
            Division = new List<Condition> { new Condition("Division", "A") }
        };

        var existingSettings = new List<LeagueSettingInfoSender>
        {
            new LeagueSettingInfoSender
            {
                LeagueSettingId = 2,
                FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition
                {
                    HasIpTv = false,
                    HasTv = true,
                    Round = new List<Condition> { new Condition("Round", "2") },
                    Division = new List<Condition> { new Condition("Division", "B") }
                })
            }
        };

        repository.QueryLeagueSetting(Arg.Any<LeagueParamRequest>()).Returns(existingSettings);

        // Act
        var result = await service.GetDuplicatedSettingId(model);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetDuplicatedSettingId_WithExactDuplicate_ShouldReturnTrue()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var service = new DuplicateSettingServiceImp(repository);

        var model = new LeagueSettingRequest
        {
            LeagueSettingId = 1,
            League = "Test League",
            HasIpTv = true,
            HasTv = false,
            Round = new List<Condition> { new Condition("Round", "1") },
            Division = new List<Condition> { new Condition("Division", "A") }
        };

        var existingSettings = new List<LeagueSettingInfoSender>
        {
            new LeagueSettingInfoSender
            {
                LeagueSettingId = 2,
                FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition
                {
                    HasIpTv = true,
                    HasTv = false,
                    Round = new List<Condition> { new Condition("Round", "1") },
                    Division = new List<Condition> { new Condition("Division", "A") }
                })
            }
        };

        repository.QueryLeagueSetting(Arg.Any<LeagueParamRequest>()).Returns(existingSettings);

        // Act
        var result = await service.GetDuplicatedSettingId(model);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetDuplicatedSettingId_WithSameLeagueSettingId_ShouldReturnFalse()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var service = new DuplicateSettingServiceImp(repository);

        var model = new LeagueSettingRequest
        {
            LeagueSettingId = 1,
            League = "Test League",
            HasIpTv = true,
            HasTv = false,
            Round = new List<Condition> { new Condition("Round", "1") },
            Division = new List<Condition> { new Condition("Division", "A") }
        };

        var existingSettings = new List<LeagueSettingInfoSender>
        {
            new LeagueSettingInfoSender
            {
                LeagueSettingId = 1, // Same ID
                FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition
                {
                    HasIpTv = true,
                    HasTv = false,
                    Round = new List<Condition> { new Condition("Round", "1") },
                    Division = new List<Condition> { new Condition("Division", "A") }
                })
            }
        };

        repository.QueryLeagueSetting(Arg.Any<LeagueParamRequest>()).Returns(existingSettings);

        // Act
        var result = await service.GetDuplicatedSettingId(model);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetDuplicatedSettingId_WithDifferentConditions_ShouldReturnFalse()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var service = new DuplicateSettingServiceImp(repository);

        var model = new LeagueSettingRequest
        {
            LeagueSettingId = 1,
            League = "Test League",
            HasIpTv = true,
            HasTv = false,
            Round = new List<Condition> { new Condition("Round", "1") },
            Division = new List<Condition> { new Condition("Division", "A") }
        };

        var existingSettings = new List<LeagueSettingInfoSender>
        {
            new LeagueSettingInfoSender
            {
                LeagueSettingId = 2,
                FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition
                {
                    HasIpTv = true,
                    HasTv = false,
                    Round = new List<Condition> { new Condition("Round", "2") }, // Different round
                    Division = new List<Condition> { new Condition("Division", "A") }
                })
            }
        };

        repository.QueryLeagueSetting(Arg.Any<LeagueParamRequest>()).Returns(existingSettings);

        // Act
        var result = await service.GetDuplicatedSettingId(model);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetDuplicatedSettingId_WithEmptyExistingSettings_ShouldReturnFalse()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var service = new DuplicateSettingServiceImp(repository);

        var model = new LeagueSettingRequest
        {
            LeagueSettingId = 1,
            League = "Test League",
            HasIpTv = true,
            HasTv = false,
            Round = new List<Condition> { new Condition("Round", "1") },
            Division = new List<Condition> { new Condition("Division", "A") }
        };

        var existingSettings = new List<LeagueSettingInfoSender>();

        repository.QueryLeagueSetting(Arg.Any<LeagueParamRequest>()).Returns(existingSettings);

        // Act
        var result = await service.GetDuplicatedSettingId(model);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsConditionsEqual_WithIdenticalConditions_ShouldReturnTrue()
    {
        // Arrange
        var first = new List<Condition>
        {
            new Condition("Type1", "Value1"),
            new Condition("Type2", "Value2")
        };

        var second = new List<Condition>
        {
            new Condition("Type1", "Value1"),
            new Condition("Type2", "Value2")
        };

        // Act
        var result = DuplicateSettingServiceImp.IsConditionsEqual(first, second);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsConditionsEqual_WithDifferentOrder_ShouldReturnTrue()
    {
        // Arrange
        var first = new List<Condition>
        {
            new Condition("Type1", "Value1"),
            new Condition("Type2", "Value2")
        };

        var second = new List<Condition>
        {
            new Condition("Type2", "Value2"),
            new Condition("Type1", "Value1")
        };

        // Act
        var result = DuplicateSettingServiceImp.IsConditionsEqual(first, second);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsConditionsEqual_WithDifferentCounts_ShouldReturnFalse()
    {
        // Arrange
        var first = new List<Condition>
        {
            new Condition("Type1", "Value1"),
            new Condition("Type2", "Value2")
        };

        var second = new List<Condition>
        {
            new Condition("Type1", "Value1")
        };

        // Act
        var result = DuplicateSettingServiceImp.IsConditionsEqual(first, second);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsConditionsEqual_WithDifferentConditions_ShouldReturnFalse()
    {
        // Arrange
        var first = new List<Condition>
        {
            new Condition("Type1", "Value1"),
            new Condition("Type2", "Value2")
        };

        var second = new List<Condition>
        {
            new Condition("Type1", "Value1"),
            new Condition("Type3", "Value3")
        };

        // Act
        var result = DuplicateSettingServiceImp.IsConditionsEqual(first, second);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsConditionsEqual_WithEmptyLists_ShouldReturnTrue()
    {
        // Arrange
        var first = new List<Condition>();
        var second = new List<Condition>();

        // Act
        var result = DuplicateSettingServiceImp.IsConditionsEqual(first, second);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsConditionsEqual_WithOneEmptyList_ShouldReturnFalse()
    {
        // Arrange
        var first = new List<Condition>();
        var second = new List<Condition>
        {
            new Condition("Type1", "Value1")
        };

        // Act
        var result = DuplicateSettingServiceImp.IsConditionsEqual(first, second);

        // Assert
        Assert.False(result);
    }
}
