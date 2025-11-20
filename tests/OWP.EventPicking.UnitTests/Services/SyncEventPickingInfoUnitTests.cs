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
    public async Task Execute_WithEmptyLeagueSettings_ShouldSetDefaultNoOfferLive()
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

        // Assert
        Assert.Equal(NoOfferConstant.None, matches[0].NoOfferLive);
        Assert.False(matches[0].IsNoOffer);
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
            CreateTestLeagueSetting(1, NoOfferConstant.Weekend),
            CreateTestLeagueSetting(1, NoOfferConstant.WeekendPH),
            CreateTestLeagueSetting(1, NoOfferConstant.WeekdayAndWeekend)
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());

        // Act
        await _service.Execute(matches);

        // Assert - Should apply WeekendPH (most restrictive)
        Assert.Equal(NoOfferConstant.WeekendPH, matches[0].NoOfferLive);
    }

    [Fact]
    public void ShouldSetNoOffer_WithWeekdayAndWeekend_ShouldReturnTrue()
    {
        // Arrange
        var matchTime = new DateTime(2024, 1, 15, 10, 0, 0); // Monday

        // Act
        var result = GetShouldSetNoOfferResult(NoOfferConstant.WeekdayAndWeekend, matchTime);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ShouldSetNoOffer_WithWeekend_ShouldReturnTrueForWeekend()
    {
        // Arrange
        var weekendTime = new DateTime(2024, 1, 20, 15, 0, 0); // Saturday
        var weekdayTime = new DateTime(2024, 1, 15, 15, 0, 0); // Monday

        // Act
        var weekendResult = GetShouldSetNoOfferResult(NoOfferConstant.Weekend, weekendTime);
        var weekdayResult = GetShouldSetNoOfferResult(NoOfferConstant.Weekend, weekdayTime);

        // Assert
        Assert.True(weekendResult);
        Assert.False(weekdayResult);
    }

    [Fact]
    public void ShouldSetNoOffer_WithWeekendPH_ShouldReturnTrueForWeekendPeakHours()
    {
        // Arrange
        var weekendPeakTime = new DateTime(2024, 1, 20, 22, 0, 0); // Saturday 10 PM
        var weekendNonPeakTime = new DateTime(2024, 1, 20, 15, 0, 0); // Saturday 3 PM
        var weekdayPeakTime = new DateTime(2024, 1, 15, 22, 0, 0); // Monday 10 PM

        // Act
        var peakResult = GetShouldSetNoOfferResult(NoOfferConstant.WeekendPH, weekendPeakTime);
        var nonPeakResult = GetShouldSetNoOfferResult(NoOfferConstant.WeekendPH, weekendNonPeakTime);
        var weekdayResult = GetShouldSetNoOfferResult(NoOfferConstant.WeekendPH, weekdayPeakTime);

        // Assert
        Assert.True(peakResult);
        Assert.False(nonPeakResult);
        Assert.False(weekdayResult);
    }

    [Fact]
    public void ShouldSetNoOffer_WithNone_ShouldReturnFalse()
    {
        // Arrange
        var matchTime = new DateTime(2024, 1, 20, 22, 0, 0); // Saturday peak hour

        // Act
        var result = GetShouldSetNoOfferResult(NoOfferConstant.None, matchTime);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsWeekendMatch_ShouldReturnTrueForSaturdayAndSunday()
    {
        // Arrange
        var saturday = new DateTime(2024, 1, 20, 15, 0, 0); // Saturday
        var sunday = new DateTime(2024, 1, 21, 15, 0, 0); // Sunday
        var monday = new DateTime(2024, 1, 22, 15, 0, 0); // Monday

        // Act
        var saturdayResult = IsWeekendMatch(saturday);
        var sundayResult = IsWeekendMatch(sunday);
        var mondayResult = IsWeekendMatch(monday);

        // Assert
        Assert.True(saturdayResult);
        Assert.True(sundayResult);
        Assert.False(mondayResult);
    }

    [Fact]
    public void IsWeekendMatch_ShouldHandleEarlyMorningMatches()
    {
        // Arrange - Match starts before noon but is considered Saturday
        var earlySaturday = new DateTime(2024, 1, 20, 2, 0, 0); // Saturday 2 AM
        var lateFriday = new DateTime(2024, 1, 19, 23, 0, 0); // Friday 11 PM (should be Saturday)

        // Act
        var earlySaturdayResult = IsWeekendMatch(earlySaturday);
        var lateFridayResult = IsWeekendMatch(lateFriday);

        // Assert
        Assert.True(earlySaturdayResult);
        Assert.True(lateFridayResult); // Late Friday night should be considered Saturday
    }

    [Fact]
    public void IsWeekendPeakHourMatch_ShouldReturnTrueForWeekendPeakHours()
    {
        // Arrange
        var peakHour1 = new DateTime(2024, 1, 20, 20, 0, 0); // Saturday 8 PM
        var peakHour2 = new DateTime(2024, 1, 20, 2, 30, 0); // Saturday 2:30 AM
        var nonPeakHour = new DateTime(2024, 1, 20, 15, 0, 0); // Saturday 3 PM
        var weekdayPeakHour = new DateTime(2024, 1, 15, 20, 0, 0); // Monday 8 PM

        // Act
        var peakResult1 = IsWeekendPeakHourMatch(peakHour1);
        var peakResult2 = IsWeekendPeakHourMatch(peakHour2);
        var nonPeakResult = IsWeekendPeakHourMatch(nonPeakHour);
        var weekdayResult = IsWeekendPeakHourMatch(weekdayPeakHour);

        // Assert
        Assert.True(peakResult1);
        Assert.True(peakResult2);
        Assert.False(nonPeakResult);
        Assert.False(weekdayResult);
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
        Assert.Equal(0, matches[2].NoOfferLive); // Unchanged
        Assert.Equal(NoOfferConstant.None, matches[3].NoOfferLive); // Default
    }

    #region Helper Methods

    private static EventPickingModel CreateTestMatch(bool? isNoOffer, int leagueId)
    {
        return new EventPickingModel
        {
            InfoSiteMatchId = 1,
            OWPLeagueId = leagueId,
            IsNoOffer = isNoOffer,
            EventDateTime = new DateTime(2024, 1, 20, 15, 0, 0), // Saturday 3 PM
            NoOfferLive = 0
        };
    }

    private static LeagueSettingInfoSender CreateTestLeagueSetting(int leagueId, int noOfferLive)
    {
        return new LeagueSettingInfoSender
        {
            OWPLeagueId = leagueId,
            NoOfferLive = noOfferLive,
            FullConditionDescription = "{}"
        };
    }

    // Helper methods to access private methods for testing
    private bool GetShouldSetNoOfferResult(int noOfferLive, DateTime eventDateTime)
    {
        var method = typeof(SyncEventPickingInfoImp).GetMethod("ShouldSetNoOffer",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var instance = new SyncEventPickingInfoImp(Substitute.For<OWP.EventPicking.Repository>());
        return (bool)method.Invoke(instance, new object[] { noOfferLive, eventDateTime });
    }

    private bool IsWeekendMatch(DateTime matchTime)
    {
        var method = typeof(SyncEventPickingInfoImp).GetMethod("IsWeekendMatch",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var instance = new SyncEventPickingInfoImp(Substitute.For<OWP.EventPicking.Repository>());
        return (bool)method.Invoke(instance, new object[] { matchTime });
    }

    private bool IsWeekendPeakHourMatch(DateTime matchTime)
    {
        var method = typeof(SyncEventPickingInfoImp).GetMethod("IsWeekendPeakHourMatch",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var instance = new SyncEventPickingInfoImp(Substitute.For<OWP.EventPicking.Repository>());
        return (bool)method.Invoke(instance, new object[] { matchTime });
    }

    #endregion
}
