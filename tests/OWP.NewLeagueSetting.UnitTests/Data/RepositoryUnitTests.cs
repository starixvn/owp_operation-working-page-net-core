using Fanex.Data.Repository;
using NSubstitute;
using OWP.NewLeagueSetting.Criteria;
using OWP.NewLeagueSetting.Data;
using OWP.NewLeagueSetting.Models;
using Xunit;

namespace OWP.NewLeagueSetting.UnitTests.Data;

public class RepositoryUnitTests
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly RepositoryImp repository;

    public RepositoryUnitTests()
    {
        dynamicRepository = Substitute.For<IDynamicRepository>();
        repository = new RepositoryImp(dynamicRepository);
    }

    [Fact]
    public async Task QueryLeagueSetting_ShouldSortResultsCorrectly()
    {
        // Arrange
        var testData = new List<LeagueSettingInfoSender>
        {
            new LeagueSettingInfoSender { LeagueSettingId = 1, League = "Z League", LeagueGroup = "Other" },
            new LeagueSettingInfoSender { LeagueSettingId = 2, League = "**A League", LeagueGroup = "Other" },
            new LeagueSettingInfoSender { LeagueSettingId = 3, League = "*B League", LeagueGroup = "Other" },
            new LeagueSettingInfoSender { LeagueSettingId = 4, League = "MJ League", LeagueGroup = "MJ" },
            new LeagueSettingInfoSender { LeagueSettingId = 5, League = "C League", LeagueGroup = "Other" }
        };

        var request = new LeagueParamRequest
        {
            LeagueName = "",
            IsEqualLeague = false
        };

        dynamicRepository.FetchAsync<LeagueSettingInfoSender>(Arg.Any<QueryLeagueSettingCriteria>())
            .Returns(testData);

        // Act
        var result = await repository.QueryLeagueSetting(request);

        // Assert
        var resultList = result.ToList();

        // MJ items should come first
        Assert.Equal("MJ", resultList[0].LeagueGroup);
        Assert.Equal("MJ League", resultList[0].League);

        // Remaining items should be sorted alphabetically after normalization
        // A League (after removing **), B League (after removing *), C League, Z League
        Assert.Equal("A League", resultList[1].League.TrimStart('*').Trim());
        Assert.Equal("B League", resultList[2].League.TrimStart('*').Trim());
        Assert.Equal("C League", resultList[3].League.TrimStart('*').Trim());
        Assert.Equal("Z League", resultList[4].League.TrimStart('*').Trim());
    }

    [Fact]
    public async Task QueryLeagueSetting_WithMultipleMJItems_ShouldSortMJItemsAlphabetically()
    {
        // Arrange
        var testData = new List<LeagueSettingInfoSender>
        {
            new LeagueSettingInfoSender { LeagueSettingId = 1, League = "Z MJ League", LeagueGroup = "MJ" },
            new LeagueSettingInfoSender { LeagueSettingId = 2, League = "A MJ League", LeagueGroup = "MJ" },
            new LeagueSettingInfoSender { LeagueSettingId = 3, League = "B MJ League", LeagueGroup = "MJ" }
        };

        var request = new LeagueParamRequest
        {
            LeagueName = "",
            IsEqualLeague = false
        };

        dynamicRepository.FetchAsync<LeagueSettingInfoSender>(Arg.Any<QueryLeagueSettingCriteria>())
            .Returns(testData);

        // Act
        var result = await repository.QueryLeagueSetting(request);

        // Assert
        var resultList = result.ToList();

        // All items are MJ, so they should be sorted alphabetically
        Assert.Equal("A MJ League", resultList[0].League);
        Assert.Equal("B MJ League", resultList[1].League);
        Assert.Equal("Z MJ League", resultList[2].League);
    }

    [Fact]
    public async Task QueryLeagueSetting_WithEmptyResult_ShouldReturnEmpty()
    {
        // Arrange
        var testData = new List<LeagueSettingInfoSender>();
        var request = new LeagueParamRequest
        {
            LeagueName = "",
            IsEqualLeague = false
        };

        dynamicRepository.FetchAsync<LeagueSettingInfoSender>(Arg.Any<QueryLeagueSettingCriteria>())
            .Returns(testData);

        // Act
        var result = await repository.QueryLeagueSetting(request);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task QueryLeagueSetting_WithNullLeagueNames_ShouldHandleGracefully()
    {
        // Arrange
        var testData = new List<LeagueSettingInfoSender>
        {
            new LeagueSettingInfoSender { LeagueSettingId = 1, League = null, LeagueGroup = "Other" },
            new LeagueSettingInfoSender { LeagueSettingId = 2, League = "", LeagueGroup = "MJ" },
            new LeagueSettingInfoSender { LeagueSettingId = 3, League = "Valid League", LeagueGroup = "Other" }
        };

        var request = new LeagueParamRequest
        {
            LeagueName = "",
            IsEqualLeague = false
        };

        dynamicRepository.FetchAsync<LeagueSettingInfoSender>(Arg.Any<QueryLeagueSettingCriteria>())
            .Returns(testData);

        // Act
        var result = await repository.QueryLeagueSetting(request);

        // Assert
        var resultList = result.ToList();

        // MJ items should come first (even with null/empty league names)
        Assert.Equal("MJ", resultList[0].LeagueGroup);
        Assert.Equal("", resultList[0].League);

        // Null league name should be handled
        Assert.Null(resultList[1].League);

        // Valid league should come last
        Assert.Equal("Valid League", resultList[2].League);
    }

    [Fact]
    public async Task QueryLeagueSetting_WithComplexAsteriskPatterns_ShouldNormalizeCorrectly()
    {
        // Arrange
        var testData = new List<LeagueSettingInfoSender>
        {
            new LeagueSettingInfoSender { LeagueSettingId = 1, League = "***League A", LeagueGroup = "Other" },
            new LeagueSettingInfoSender { LeagueSettingId = 2, League = "**League B", LeagueGroup = "Other" },
            new LeagueSettingInfoSender { LeagueSettingId = 3, League = "*League C", LeagueGroup = "Other" },
            new LeagueSettingInfoSender { LeagueSettingId = 4, League = "League D", LeagueGroup = "Other" }
        };

        var request = new LeagueParamRequest
        {
            LeagueName = "",
            IsEqualLeague = false
        };

        dynamicRepository.FetchAsync<LeagueSettingInfoSender>(Arg.Any<QueryLeagueSettingCriteria>())
            .Returns(testData);

        // Act
        var result = await repository.QueryLeagueSetting(request);

        // Assert
        var resultList = result.ToList();

        // All should be normalized and sorted alphabetically
        Assert.Equal("League A", resultList[0].League.TrimStart('*').Trim());
        Assert.Equal("League B", resultList[1].League.TrimStart('*').Trim());
        Assert.Equal("League C", resultList[2].League.TrimStart('*').Trim());
        Assert.Equal("League D", resultList[3].League.TrimStart('*').Trim());
    }

    [Fact]
    public async Task QueryLeagueSetting_WithLeaguesStartingWithV_ShouldSortCorrectly()
    {
        // Arrange - Test case specifically for leagues starting with 'V' after normalization
        var testData = new List<LeagueSettingInfoSender>
        {
            new LeagueSettingInfoSender { LeagueSettingId = 1, League = "Z League", LeagueGroup = "Other" },
            new LeagueSettingInfoSender { LeagueSettingId = 2, League = "**V League", LeagueGroup = "Other" }, // After normalize: "V League"
            new LeagueSettingInfoSender { LeagueSettingId = 3, League = "*A League", LeagueGroup = "Other" },  // After normalize: "A League"
            new LeagueSettingInfoSender { LeagueSettingId = 4, League = "MJ League", LeagueGroup = "MJ" },
            new LeagueSettingInfoSender { LeagueSettingId = 5, League = "B League", LeagueGroup = "Other" }
        };

        var request = new LeagueParamRequest
        {
            LeagueName = "",
            IsEqualLeague = false
        };

        dynamicRepository.FetchAsync<LeagueSettingInfoSender>(Arg.Any<QueryLeagueSettingCriteria>())
            .Returns(testData);

        // Act
        var result = await repository.QueryLeagueSetting(request);

        // Assert
        var resultList = result.ToList();

        // MJ items should come first
        Assert.Equal("MJ", resultList[0].LeagueGroup);
        Assert.Equal("MJ League", resultList[0].League);

        // Remaining items should be sorted alphabetically after normalization
        // A League (after removing *), B League, V League (after removing **), Z League
        Assert.Equal("A League", resultList[1].League.TrimStart('*').Trim());
        Assert.Equal("B League", resultList[2].League.TrimStart('*').Trim());
        Assert.Equal("V League", resultList[3].League.TrimStart('*').Trim());
        Assert.Equal("Z League", resultList[4].League.TrimStart('*').Trim());

        // Verify the actual normalized values for sorting
        var normalizedNames = resultList.Skip(1).Select(x => NormalizeLeagueName(x.League)).ToList();
        Assert.Equal("A League", normalizedNames[0]);
        Assert.Equal("B League", normalizedNames[1]);
        Assert.Equal("V League", normalizedNames[2]);
        Assert.Equal("Z League", normalizedNames[3]);
    }

    [Fact]
    public async Task QueryLeagueSetting_WithRealWorldExample_ShouldSortCorrectly()
    {
        // Arrange - Real-world example from user's issue
        var testData = new List<LeagueSettingInfoSender>
        {
            new LeagueSettingInfoSender { LeagueSettingId = 1, League = "**Virtual Fifa World Cup 2022 Group H (In Qatar) PES 21 (1st-15th July) - 20 Mins Play", LeagueGroup = "Other" },
            new LeagueSettingInfoSender { LeagueSettingId = 2, League = "A - ALL IN ONE NUMBER OF CORNERS", LeagueGroup = "Other" },
            new LeagueSettingInfoSender { LeagueSettingId = 3, League = "MJ League", LeagueGroup = "MJ" },
            new LeagueSettingInfoSender { LeagueSettingId = 4, League = "B League", LeagueGroup = "Other" }
        };

        var request = new LeagueParamRequest
        {
            LeagueName = "",
            IsEqualLeague = false
        };

        dynamicRepository.FetchAsync<LeagueSettingInfoSender>(Arg.Any<QueryLeagueSettingCriteria>())
            .Returns(testData);

        // Act
        var result = await repository.QueryLeagueSetting(request);

        // Assert
        var resultList = result.ToList();

        // MJ items should come first
        Assert.Equal("MJ", resultList[0].LeagueGroup);
        Assert.Equal("MJ League", resultList[0].League);

        // Remaining items should be sorted alphabetically after normalization
        // A - ALL IN ONE NUMBER OF CORNERS, B League, Virtual Fifa World Cup...
        var normalizedNames = resultList.Skip(1).Select(x => NormalizeLeagueName(x.League)).ToList();

        // Verify the order: A should come before V
        Assert.Equal("A - ALL IN ONE NUMBER OF CORNERS", normalizedNames[0]);
        Assert.Equal("B League", normalizedNames[1]);
        Assert.Equal("Virtual Fifa World Cup 2022 Group H (In Qatar) PES 21 (1st-15th July) - 20 Mins Play", normalizedNames[2]);

        // Double check the actual league names in result
        Assert.Equal("A - ALL IN ONE NUMBER OF CORNERS", resultList[1].League);
        Assert.Equal("B League", resultList[2].League);
        Assert.Equal("**Virtual Fifa World Cup 2022 Group H (In Qatar) PES 21 (1st-15th July) - 20 Mins Play", resultList[3].League);
    }

    private static string NormalizeLeagueName(string leagueName)
    {
        if (string.IsNullOrWhiteSpace(leagueName))
            return string.Empty;

        // Remove leading asterisks and trim
        return leagueName.TrimStart('*').Trim();
    }
}
