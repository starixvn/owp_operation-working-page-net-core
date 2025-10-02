using OWP.Api.Trader.Model;

namespace OWP.Api.UnitTests.Models;

public class TraderModelsUnitTests
{
    [Fact]
    public void Request_DefaultConstructor_InitializesProperties()
    {
        // Act
        var request = new Request();

        // Assert
        Assert.Null(request.FromDate);
        Assert.Null(request.ToDate);
        Assert.Null(request.Matches);
    }

    [Fact]
    public void Request_WithValues_SetsPropertiesCorrectly()
    {
        // Arrange
        var fromDate = new DateTime(2024, 1, 1);
        var toDate = new DateTime(2024, 1, 31);
        var matches = new List<MatchRequest>
        {
            new() { MatchId = 123, ParentId = 0, Subtype = 1 },
            new() { MatchId = 456, ParentId = 123, Subtype = 2 }
        };

        // Act
        var request = new Request
        {
            FromDate = fromDate,
            ToDate = toDate,
            Matches = matches
        };

        // Assert
        Assert.Equal(fromDate, request.FromDate);
        Assert.Equal(toDate, request.ToDate);
        Assert.Equal(matches.Count, request.Matches.Count);
        Assert.Equal(matches[0].MatchId, request.Matches[0].MatchId);
        Assert.Equal(matches[1].MatchId, request.Matches[1].MatchId);
    }

    [Fact]
    public void MatchRequest_DefaultConstructor_InitializesProperties()
    {
        // Act
        var matchRequest = new MatchRequest();

        // Assert
        Assert.Equal(0, matchRequest.MatchId);
        Assert.Equal(0, matchRequest.ParentId);
        Assert.Equal(0, matchRequest.Subtype);
    }

    [Fact]
    public void MatchRequest_WithValues_SetsPropertiesCorrectly()
    {
        // Arrange
        var matchId = 123;
        var parentId = 456;
        var subtype = 789;

        // Act
        var matchRequest = new MatchRequest
        {
            MatchId = matchId,
            ParentId = parentId,
            Subtype = subtype
        };

        // Assert
        Assert.Equal(matchId, matchRequest.MatchId);
        Assert.Equal(parentId, matchRequest.ParentId);
        Assert.Equal(subtype, matchRequest.Subtype);
    }

    [Fact]
    public void OWPMatchRequest_DefaultConstructor_InitializesProperties()
    {
        // Act
        var owpMatchRequest = new OWPMatchRequest();

        // Assert
        Assert.Equal(0, owpMatchRequest.MatchId);
        Assert.Equal(0, owpMatchRequest.ParentId);
        Assert.Equal(0, owpMatchRequest.Subtype);
        Assert.Null(owpMatchRequest.MinorBettypeId);
        Assert.False(owpMatchRequest.IsMainMatch);
    }

    [Fact]
    public void OWPMatchRequest_WithValues_SetsPropertiesCorrectly()
    {
        // Arrange
        var matchId = 123;
        var parentId = 456;
        var subtype = 789;
        var minorBettypeId = 101;
        var isMainMatch = true;

        // Act
        var owpMatchRequest = new OWPMatchRequest
        {
            MatchId = matchId,
            ParentId = parentId,
            Subtype = subtype,
            MinorBettypeId = minorBettypeId,
            IsMainMatch = isMainMatch
        };

        // Assert
        Assert.Equal(matchId, owpMatchRequest.MatchId);
        Assert.Equal(parentId, owpMatchRequest.ParentId);
        Assert.Equal(subtype, owpMatchRequest.Subtype);
        Assert.Equal(minorBettypeId, owpMatchRequest.MinorBettypeId);
        Assert.Equal(isMainMatch, owpMatchRequest.IsMainMatch);
    }

    [Fact]
    public void OWPMatchRequest_WithNullMinorBettypeId_SetsMinorBettypeIdToNull()
    {
        // Arrange
        var owpMatchRequest = new OWPMatchRequest
        {
            MatchId = 123,
            ParentId = 456,
            Subtype = 789,
            MinorBettypeId = null,
            IsMainMatch = false
        };

        // Assert
        Assert.Null(owpMatchRequest.MinorBettypeId);
    }

    [Fact]
    public void OWPAssignment_DefaultConstructor_InitializesProperties()
    {
        // Act
        var owpAssignment = new OWPAssignment();

        // Assert
        Assert.Equal(0, owpAssignment.MatchId);
        Assert.Equal(0, owpAssignment.UserId);
        Assert.Equal(0, owpAssignment.Bettype);
    }

    [Fact]
    public void OWPAssignment_WithValues_SetsPropertiesCorrectly()
    {
        // Arrange
        var matchId = 123;
        var userId = 456;
        var bettype = 789;

        // Act
        var owpAssignment = new OWPAssignment
        {
            MatchId = matchId,
            UserId = userId,
            Bettype = bettype
        };

        // Assert
        Assert.Equal(matchId, owpAssignment.MatchId);
        Assert.Equal(userId, owpAssignment.UserId);
        Assert.Equal(bettype, owpAssignment.Bettype);
    }

    [Fact]
    public void TraderMatch_DefaultConstructor_InitializesProperties()
    {
        // Act
        var traderMatch = new TraderMatch();

        // Assert
        Assert.Equal(0, traderMatch.MatchId);
        Assert.Null(traderMatch.Traders);
    }

    [Fact]
    public void TraderMatch_WithValues_SetsPropertiesCorrectly()
    {
        // Arrange
        var matchId = 123;
        var traders = new List<AssignedTrader>
        {
            new() { Bettype = 1, TraderId = 100 },
            new() { Bettype = 2, TraderId = 101 }
        };

        // Act
        var traderMatch = new TraderMatch
        {
            MatchId = matchId,
            Traders = traders
        };

        // Assert
        Assert.Equal(matchId, traderMatch.MatchId);
        Assert.Equal(traders.Count, traderMatch.Traders.Count);
        Assert.Equal(traders[0].Bettype, traderMatch.Traders[0].Bettype);
        Assert.Equal(traders[0].TraderId, traderMatch.Traders[0].TraderId);
        Assert.Equal(traders[1].Bettype, traderMatch.Traders[1].Bettype);
        Assert.Equal(traders[1].TraderId, traderMatch.Traders[1].TraderId);
    }

    [Fact]
    public void AssignedTrader_DefaultConstructor_InitializesProperties()
    {
        // Act
        var assignedTrader = new AssignedTrader();

        // Assert
        Assert.Equal(0, assignedTrader.Bettype);
        Assert.Equal(0, assignedTrader.TraderId);
    }

    [Fact]
    public void AssignedTrader_WithValues_SetsPropertiesCorrectly()
    {
        // Arrange
        var bettype = 123;
        var traderId = 456;

        // Act
        var assignedTrader = new AssignedTrader
        {
            Bettype = bettype,
            TraderId = traderId
        };

        // Assert
        Assert.Equal(bettype, assignedTrader.Bettype);
        Assert.Equal(traderId, assignedTrader.TraderId);
    }

    [Theory]
    [InlineData(0, 0, 0)] // All zeros
    [InlineData(1, 2, 3)] // Positive values
    [InlineData(-1, -2, -3)] // Negative values
    [InlineData(int.MaxValue, int.MaxValue, int.MaxValue)] // Max values
    [InlineData(int.MinValue, int.MinValue, int.MinValue)] // Min values
    public void MatchRequest_WithVariousValues_SetsPropertiesCorrectly(int matchId, int parentId, int subtype)
    {
        // Act
        var matchRequest = new MatchRequest
        {
            MatchId = matchId,
            ParentId = parentId,
            Subtype = subtype
        };

        // Assert
        Assert.Equal(matchId, matchRequest.MatchId);
        Assert.Equal(parentId, matchRequest.ParentId);
        Assert.Equal(subtype, matchRequest.Subtype);
    }
}
