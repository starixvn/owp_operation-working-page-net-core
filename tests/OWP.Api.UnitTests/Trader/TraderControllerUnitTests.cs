using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using OWP.Api.Trader;
using OWP.Api.Trader.Handlers;
using OWP.Api.Trader.Model;

namespace OWP.Api.UnitTests.Trader;

public class TraderControllerUnitTests
{
    private readonly IMediator _mediator;
    private readonly TraderController _controller;

    public TraderControllerUnitTests()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new TraderController(_mediator);
    }

    [Fact]
    public async Task GetMaxbet_WithValidMatches_ReturnsOkWithTraderMatches()
    {
        // Arrange
        var matches = new List<MatchRequest>
        {
            new() { MatchId = 123, ParentId = 0, Subtype = 1 },
            new() { MatchId = 456, ParentId = 123, Subtype = 2 }
        };

        var expectedTraderMatches = new List<TraderMatch>
        {
            new()
            {
                MatchId = 123,
                Traders = new List<AssignedTrader>
                {
                    new() { Bettype = 1, TraderId = 100 },
                    new() { Bettype = 2, TraderId = 101 }
                }
            },
            new()
            {
                MatchId = 456,
                Traders = new List<AssignedTrader>
                {
                    new() { Bettype = 3, TraderId = 102 }
                }
            }
        };

        _mediator.Send(Arg.Any<TraderRequest>()).Returns(expectedTraderMatches);

        // Act
        var result = await _controller.GetMaxbet(matches);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedTraderMatches = Assert.IsType<List<TraderMatch>>(okResult.Value);

        Assert.Equal(expectedTraderMatches.Count, returnedTraderMatches.Count);

        for (int i = 0; i < expectedTraderMatches.Count; i++)
        {
            Assert.Equal(expectedTraderMatches[i].MatchId, returnedTraderMatches[i].MatchId);
            Assert.Equal(expectedTraderMatches[i].Traders.Count, returnedTraderMatches[i].Traders.Count);
        }

        await _mediator.Received(1).Send(Arg.Is<TraderRequest>(req =>
            req.Matches.Count == matches.Count &&
            req.Matches[0].MatchId == matches[0].MatchId &&
            req.Matches[1].MatchId == matches[1].MatchId));
    }

    [Fact]
    public async Task GetMaxbet_WithEmptyMatches_ReturnsOkWithEmptyResult()
    {
        // Arrange
        var matches = new List<MatchRequest>();
        var expectedTraderMatches = new List<TraderMatch>();

        _mediator.Send(Arg.Any<TraderRequest>()).Returns(expectedTraderMatches);

        // Act
        var result = await _controller.GetMaxbet(matches);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedTraderMatches = Assert.IsType<List<TraderMatch>>(okResult.Value);
        Assert.Empty(returnedTraderMatches);

        await _mediator.Received(1).Send(Arg.Is<TraderRequest>(req => req.Matches.Count == 0));
    }

    [Fact]
    public async Task GetMaxbet_WithNullMatches_ReturnsOkWithEmptyResult()
    {
        // Arrange
        List<MatchRequest> matches = null!;
        var expectedTraderMatches = new List<TraderMatch>();

        _mediator.Send(Arg.Any<TraderRequest>()).Returns(expectedTraderMatches);

        // Act
        var result = await _controller.GetMaxbet(matches);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedTraderMatches = Assert.IsType<List<TraderMatch>>(okResult.Value);
        Assert.Empty(returnedTraderMatches);

        await _mediator.Received(1).Send(Arg.Is<TraderRequest>(req => req.Matches == null));
    }

    [Fact]
    public async Task GetTraderByTimeRange_WithValidRequest_ReturnsOkWithTraderMatches()
    {
        // Arrange
        var request = new Request
        {
            FromDate = new DateTime(2024, 1, 1),
            ToDate = new DateTime(2024, 1, 31),
            Matches = new List<MatchRequest>
            {
                new() { MatchId = 123, ParentId = 0, Subtype = 1 },
                new() { MatchId = 456, ParentId = 123, Subtype = 2 }
            }
        };

        var expectedTraderMatches = new List<TraderMatch>
        {
            new()
            {
                MatchId = 123,
                Traders = new List<AssignedTrader>
                {
                    new() { Bettype = 1, TraderId = 100 }
                }
            }
        };

        _mediator.Send(Arg.Any<TraderRequestByTimeRange>()).Returns(expectedTraderMatches);

        // Act
        var result = await _controller.GetTraderByTimeRange(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedTraderMatches = Assert.IsType<List<TraderMatch>>(okResult.Value);

        Assert.Equal(expectedTraderMatches.Count, returnedTraderMatches.Count);
        Assert.Equal(expectedTraderMatches[0].MatchId, returnedTraderMatches[0].MatchId);

        await _mediator.Received(1).Send(Arg.Is<TraderRequestByTimeRange>(req =>
            req.Matches.Count == request.Matches.Count &&
            req.FromDate == request.FromDate &&
            req.ToDate == request.ToDate));
    }

    [Fact]
    public async Task GetTraderByTimeRange_WithNullDates_ReturnsOkWithTraderMatches()
    {
        // Arrange
        var request = new Request
        {
            FromDate = null,
            ToDate = null,
            Matches = new List<MatchRequest>
            {
                new() { MatchId = 123, ParentId = 0, Subtype = 1 }
            }
        };

        var expectedTraderMatches = new List<TraderMatch>();

        _mediator.Send(Arg.Any<TraderRequestByTimeRange>()).Returns(expectedTraderMatches);

        // Act
        var result = await _controller.GetTraderByTimeRange(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedTraderMatches = Assert.IsType<List<TraderMatch>>(okResult.Value);
        Assert.Empty(returnedTraderMatches);

        await _mediator.Received(1).Send(Arg.Is<TraderRequestByTimeRange>(req =>
            req.FromDate == null &&
            req.ToDate == null &&
            req.Matches.Count == 1));
    }

    [Fact]
    public async Task GetTraderByTimeRange_WithEmptyMatches_ReturnsOkWithEmptyResult()
    {
        // Arrange
        var request = new Request
        {
            FromDate = new DateTime(2024, 1, 1),
            ToDate = new DateTime(2024, 1, 31),
            Matches = new List<MatchRequest>()
        };

        var expectedTraderMatches = new List<TraderMatch>();

        _mediator.Send(Arg.Any<TraderRequestByTimeRange>()).Returns(expectedTraderMatches);

        // Act
        var result = await _controller.GetTraderByTimeRange(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedTraderMatches = Assert.IsType<List<TraderMatch>>(okResult.Value);
        Assert.Empty(returnedTraderMatches);

        await _mediator.Received(1).Send(Arg.Is<TraderRequestByTimeRange>(req =>
            req.Matches.Count == 0 &&
            req.FromDate == request.FromDate &&
            req.ToDate == request.ToDate));
    }

    [Theory]
    [InlineData(1, 0, 1)] // Main match
    [InlineData(2, 1, 2)] // Sub match
    [InlineData(3, 0, 0)] // Main match with subtype 0
    public async Task GetMaxbet_WithDifferentMatchTypes_CallsMediatorWithCorrectRequest(int matchId, int parentId, int subtype)
    {
        // Arrange
        var matches = new List<MatchRequest>
        {
            new() { MatchId = matchId, ParentId = parentId, Subtype = subtype }
        };

        var expectedTraderMatches = new List<TraderMatch>();
        _mediator.Send(Arg.Any<TraderRequest>()).Returns(expectedTraderMatches);

        // Act
        await _controller.GetMaxbet(matches);

        // Assert
        await _mediator.Received(1).Send(Arg.Is<TraderRequest>(req =>
            req.Matches.Count == 1 &&
            req.Matches[0].MatchId == matchId &&
            req.Matches[0].ParentId == parentId &&
            req.Matches[0].Subtype == subtype));
    }
}
