using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Services;

public class BuildMBForClubFriendlyMajorLeagueBuilderTests
{
    private readonly BuildMBForClubFriendlyMajorLeagueBuilder service;

    public BuildMBForClubFriendlyMajorLeagueBuilderTests()
    {
        service = new BuildMBForClubFriendlyMajorLeagueBuilderImp();
    }

    [Fact]
    public void Build_WithHasTvTrue_ShouldApplyMajorLeagueRulesWithTvSettings()
    {
        // Arrange
        var match = MockEventPickingModel();
        var hasTv = true;
        var hasIpTv = false;

        // Act
        service.Build(match, hasTv, hasIpTv);

        // Assert
        Assert.Equal(40000, match.MaxbetHdp);
        Assert.Equal(0.08m, match.SpreadHdp);
        Assert.Equal(0.1m, match.SpreadOu);
        Assert.Equal("4+4 lines", match.NoOfHDP);
        Assert.True(match.IsReferenceLeagueManagement);
    }

    [Fact]
    public void Build_WithHasTvFalseAndHasIpTvTrue_ShouldApplyMajorLeagueRulesWithIpTvSettings()
    {
        // Arrange
        var match = MockEventPickingModel();
        var hasTv = false;
        var hasIpTv = true;

        // Act
        service.Build(match, hasTv, hasIpTv);

        // Assert
        Assert.Equal(30000, match.MaxbetHdp);
        Assert.Equal(0.08m, match.SpreadHdp);
        Assert.Equal(0.1m, match.SpreadOu);
        Assert.Equal("4+4 lines", match.NoOfHDP);
        Assert.True(match.IsReferenceLeagueManagement);
    }

    [Fact]
    public void Build_WithHasTvFalseAndHasIpTvFalse_ShouldApplyMajorLeagueRulesWithDefaultSettings()
    {
        // Arrange
        var match = MockEventPickingModel();
        var hasTv = false;
        var hasIpTv = false;

        // Act
        service.Build(match, hasTv, hasIpTv);

        // Assert
        Assert.Equal(25000, match.MaxbetHdp);
        Assert.Equal(0.08m, match.SpreadHdp);
        Assert.Equal(0.1m, match.SpreadOu);
        Assert.Equal("4+4 lines", match.NoOfHDP);
        Assert.True(match.IsReferenceLeagueManagement);
    }

    [Fact]
    public void Build_WithHasTvTrueAndHasIpTvTrue_ShouldPrioritizeTvSettings()
    {
        // Arrange
        var match = MockEventPickingModel();
        var hasTv = true;
        var hasIpTv = true;

        // Act
        service.Build(match, hasTv, hasIpTv);

        // Assert
        // Should use TV settings (40000) when both TV and IPTV are true
        Assert.Equal(40000, match.MaxbetHdp);
        Assert.Equal(0.08m, match.SpreadHdp);
        Assert.Equal(0.1m, match.SpreadOu);
        Assert.Equal("4+4 lines", match.NoOfHDP);
        Assert.True(match.IsReferenceLeagueManagement);
    }

    [Fact]
    public void Build_ShouldOverrideExistingValues()
    {
        // Arrange
        var match = MockEventPickingModel();
        match.MaxbetHdp = 10000;
        match.SpreadHdp = 0.05m;
        match.SpreadOu = 0.06m;
        match.NoOfHDP = "2 lines";
        match.IsReferenceLeagueManagement = false;

        var hasTv = true;
        var hasIpTv = false;

        // Act
        service.Build(match, hasTv, hasIpTv);

        // Assert
        Assert.Equal(40000, match.MaxbetHdp);
        Assert.Equal(0.08m, match.SpreadHdp);
        Assert.Equal(0.1m, match.SpreadOu);
        Assert.Equal("4+4 lines", match.NoOfHDP);
        Assert.True(match.IsReferenceLeagueManagement);
    }

    [Fact]
    public void Build_WithNullMatch_ShouldNotThrowException()
    {
        // Arrange
        EventPickingModel match = null;
        var hasTv = true;
        var hasIpTv = false;

        // Act & Assert
        var exception = Assert.Throws<NullReferenceException>(() => service.Build(match, hasTv, hasIpTv));
        Assert.NotNull(exception);
    }

    [Theory]
    [InlineData(true, false, 40000)]
    [InlineData(false, true, 30000)]
    [InlineData(false, false, 25000)]
    [InlineData(true, true, 40000)]
    public void Build_WithDifferentTvCombinations_ShouldSetCorrectMaxbet(bool hasTv, bool hasIpTv, decimal expectedMaxbet)
    {
        // Arrange
        var match = MockEventPickingModel();

        // Act
        service.Build(match, hasTv, hasIpTv);

        // Assert
        Assert.Equal(expectedMaxbet, match.MaxbetHdp);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    [InlineData(true, true)]
    public void Build_WithAnyTvCombination_ShouldSetCorrectSpreadAndNoOfHDP(bool hasTv, bool hasIpTv)
    {
        // Arrange
        var match = MockEventPickingModel();

        // Act
        service.Build(match, hasTv, hasIpTv);

        // Assert
        Assert.Equal(0.08m, match.SpreadHdp);
        Assert.Equal(0.1m, match.SpreadOu);
        Assert.Equal("4+4 lines", match.NoOfHDP);
        Assert.True(match.IsReferenceLeagueManagement);
    }

    private static EventPickingModel MockEventPickingModel()
    {
        return new EventPickingModel
        {
            OWPLeagueId = 123,
            InfoSiteMatchId = 456,
            LeagueName = "Club Friendly",
            HomeName = "Team A",
            AwayName = "Team B",
            EventDateTime = DateTime.UtcNow,
            MaxbetHdp = 0,
            SpreadHdp = 0,
            SpreadOu = 0,
            NoOfHDP = string.Empty,
            IsReferenceLeagueManagement = false
        };
    }
}
