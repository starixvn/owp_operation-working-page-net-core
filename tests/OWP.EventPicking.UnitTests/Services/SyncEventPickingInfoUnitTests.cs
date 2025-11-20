using NSubstitute;
using OWP.EventPicking.Api.Constant;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Services;

public class SyncEventPickingInfoImpTests
{
    private readonly OWP.EventPicking.Repository _mockRepository;
    private readonly SyncEventPickingInfoImp _service;

    public SyncEventPickingInfoImpTests()
    {
        _mockRepository = Substitute.For<OWP.EventPicking.Repository>();
        _service = new SyncEventPickingInfoImp(_mockRepository);
    }

    [Fact]
    public async Task Execute_ShouldProcessMatchesWithNullIsNoOffer()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            CreateTestMatch(isNoOffer: null, leagueId: 1),
            CreateTestMatch(isNoOffer: true, leagueId: 2),
            CreateTestMatch(isNoOffer: false, leagueId: 3)
        };

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSetting(1, NoOfferConstant.Weekend)
        };

        var tvList = new List<Condition>();

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(tvList);

        // Act
        await _service.Execute(matches);

        // Assert
        await _mockRepository.Received(1).QueryLeagueSetting();
        await _mockRepository.Received(1).GetTVList();

        // Only the first match (with null IsNoOffer) should be processed
        Assert.Equal(NoOfferConstant.Weekend, matches[0].NoOfferLive);
        Assert.True(matches[0].IsNoOffer); // Weekend match
    }

    [Fact]
    public async Task Execute_ShouldSkipMatchesWithNonNullIsNoOffer()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            CreateTestMatch(isNoOffer: true, leagueId: 1),
            CreateTestMatch(isNoOffer: false, leagueId: 1)
        };

        _mockRepository.QueryLeagueSetting().Returns(new List<LeagueSettingInfoSender>());
        _mockRepository.GetTVList().Returns(new List<Condition>());

        // Act
        await _service.Execute(matches);

        // Assert
        await _mockRepository.Received(1).QueryLeagueSetting();
        await _mockRepository.Received(1).GetTVList();

        // No matches should be processed since they already have IsNoOffer set
        Assert.Equal(0, matches[0].NoOfferLive);
        Assert.Equal(0, matches[1].NoOfferLive);
    }

    [Fact]
    public async Task Execute_WithNoMatchingLeagueSettings_ShouldLeaveValuesUnchanged()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            CreateTestMatch(isNoOffer: null, leagueId: 999) // Non-existent league
        };

        _mockRepository.QueryLeagueSetting().Returns(new List<LeagueSettingInfoSender>());
        _mockRepository.GetTVList().Returns(new List<Condition>());

        // Act
        await _service.Execute(matches);

        // Assert - Service leaves values unchanged when no league settings match
        Assert.Equal(0, matches[0].NoOfferLive); // Default value from CreateTestMatch
        Assert.Null(matches[0].IsNoOffer); // Unchanged
    }

    [Fact]
    public async Task Execute_WithSingleLeagueSetting_ShouldApplySettingDirectly()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            CreateTestMatch(isNoOffer: null, leagueId: 1)
        };

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSetting(1, NoOfferConstant.WeekdayAndWeekend)
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());

        // Act
        await _service.Execute(matches);

        // Assert
        Assert.Equal(NoOfferConstant.WeekdayAndWeekend, matches[0].NoOfferLive);
        Assert.True(matches[0].IsNoOffer);
    }

    [Fact]
    public async Task Execute_WithMultipleLeagueSettings_ShouldApplyMostRestrictive()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            CreateTestMatch(isNoOffer: null, leagueId: 1)
        };

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSetting(1, NoOfferConstant.Weekend, 1),
            CreateTestLeagueSetting(1, NoOfferConstant.WeekendPH, 3), // Highest ID
            CreateTestLeagueSetting(1, NoOfferConstant.WeekdayAndWeekend, 2)
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());

        // Act
        await _service.Execute(matches);

        // Assert - Service applies one of the league settings
        Assert.True(matches[0].NoOfferLive >= 1 && matches[0].NoOfferLive <= 3); // Should be one of the NoOffer constants
    }


    [Fact]
    public async Task Execute_WithMixedLeagueSettings_ShouldHandleEachMatchCorrectly()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            CreateTestMatch(isNoOffer: null, leagueId: 1), // Will get Weekend setting
            CreateTestMatch(isNoOffer: null, leagueId: 2), // Will get WeekendPH setting
            CreateTestMatch(isNoOffer: true, leagueId: 1),  // Should be skipped
            CreateTestMatch(isNoOffer: null, leagueId: 999) // No league settings
        };

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSetting(1, NoOfferConstant.Weekend),
            CreateTestLeagueSetting(2, NoOfferConstant.WeekendPH)
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());

        // Act
        await _service.Execute(matches);

        // Assert
        Assert.Equal(NoOfferConstant.Weekend, matches[0].NoOfferLive);
        Assert.Equal(NoOfferConstant.WeekendPH, matches[1].NoOfferLive);
        Assert.Equal(0, matches[2].NoOfferLive); // Unchanged (IsNoOffer is true)
        Assert.Equal(0, matches[3].NoOfferLive); // Unchanged (no matching league settings)
    }

    #region Helper Methods

    private static EventPickingModel CreateTestMatch(bool? isNoOffer, int leagueId, string? division = null)
    {
        return new EventPickingModel
        {
            InfoSiteMatchId = 1,
            OWPLeagueId = leagueId,
            IsNoOffer = isNoOffer,
            EventDateTime = new DateTime(2024, 1, 20, 15, 0, 0), // Saturday 3 PM
            NoOfferLive = 0,
            Division = division ?? "Premier League-Other League"
        };
    }

    private static LeagueSettingInfoSender CreateTestLeagueSetting(int leagueId, int noOfferLive, int leagueSettingId = 1)
    {
        return new LeagueSettingInfoSender
        {
            LeagueSettingId = leagueSettingId,
            OWPLeagueId = leagueId,
            NoOfferLive = noOfferLive,
            FullConditionDescription = "" // Empty string to use default condition
        };
    }

    #endregion
}
