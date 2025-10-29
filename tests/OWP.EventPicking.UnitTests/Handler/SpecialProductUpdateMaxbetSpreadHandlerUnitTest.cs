using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fanex.NAP.Client;
using MediatR;
using Newtonsoft.Json;
using NSubstitute;
using OWP.CQRS;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Handler;

public class SpecialProductUpdateMaxbetSpreadHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldGetCacheData_AndCallRepository_AndSaveCache_AndReturnUnit()
    {
        // Arrange
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var session = Substitute.For<INapSession>();
        var cacheManipulator = Substitute.For<CacheManipulator>();

        session.UserId.Returns(789);
        session.UserName.Returns("testuser");

        var handler = new UpdateMaxbetHandler(repository, session, cacheManipulator);

        var events = new List<UpdateEvents>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 100, SpreadHdp = 0.5m },
            new() { InfoSiteMatchId = 456, MaxbetHdp = 200, SpreadOu = 1.0m }
        };

        var request = new UpdateEventsRequest
        {
            Events = events,
            EventDate = new DateTime(2024, 1, 15)
        };

        var cachedMBSpreads = new List<EventMBSpread>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 50, SpreadHdp = 0.3m },
            new() { InfoSiteMatchId = 456, MaxbetHdp = 150, SpreadOu = 0.8m }
        };

        cacheManipulator.GetMBSpreadsByDate(request.EventDate).Returns(cachedMBSpreads);

        var expectedJson = JsonConvert.SerializeObject(events);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await cacheManipulator.Received(1).GetMBSpreadsByDate(request.EventDate);
        await repository.Received(1).UpdateEvent(
            Arg.Is<UpdateMaxbetSpreadCriteria>(c =>
                c.ActorId == 789 &&
                c.TargetJson == expectedJson));
        await repository.Received(1).SaveEventLog(
            Arg.Is<SaveEditEventLogsCriteria>(c => !string.IsNullOrEmpty(c.Xml)));
        await cacheManipulator.Received(1).SaveMBSpreadsByDate(request.EventDate,
            Arg.Is<List<EventMBSpread>>(cachedData =>
                cachedData.Count == 2 && // Should contain both original cached events
                cachedData.Any(c => c.InfoSiteMatchId == 123 && c.MaxbetHdp == 100) && // Updated event
                cachedData.Any(c => c.InfoSiteMatchId == 456 && c.MaxbetHdp == 200))); // Updated event

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldDetectChanges_AndGenerateLogData()
    {
        // Arrange
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var session = Substitute.For<INapSession>();
        var cacheManipulator = Substitute.For<CacheManipulator>();

        session.UserId.Returns(789);
        session.UserName.Returns("testuser");

        var handler = new UpdateMaxbetHandler(repository, session, cacheManipulator);

        var events = new List<UpdateEvents>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 100000, SpreadHdp = 0.5m, SpreadOu = 1.0m, NoOfHDP = "2 lines" }
        };

        var request = new UpdateEventsRequest
        {
            Events = events,
            EventDate = new DateTime(2024, 1, 15)
        };

        var cachedMBSpreads = new List<EventMBSpread>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 50000, SpreadHdp = 0.3m, SpreadOu = 0.8m, NoOfHDP = "1 line" }
        };

        cacheManipulator.GetMBSpreadsByDate(request.EventDate).Returns(cachedMBSpreads);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).SaveEventLog(
            Arg.Is<SaveEditEventLogsCriteria>(c =>
                !string.IsNullOrEmpty(c.Xml) &&
                c.Xml.Contains("MB") &&
                c.Xml.Contains("SpreadHDP") &&
                c.Xml.Contains("SpreadOU") &&
                c.Xml.Contains("No.HDP")));

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithNoChanges_ShouldNotGenerateLogData()
    {
        // Arrange
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var session = Substitute.For<INapSession>();
        var cacheManipulator = Substitute.For<CacheManipulator>();

        session.UserId.Returns(789);
        session.UserName.Returns("testuser");

        var handler = new UpdateMaxbetHandler(repository, session, cacheManipulator);

        var events = new List<UpdateEvents>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 100000, SpreadHdp = 0.5m, SpreadOu = 1.0m, NoOfHDP = "2 lines" }
        };

        var request = new UpdateEventsRequest
        {
            Events = events,
            EventDate = new DateTime(2024, 1, 15)
        };

        var cachedMBSpreads = new List<EventMBSpread>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 100000, SpreadHdp = 0.5m, SpreadOu = 1.0m, NoOfHDP = "2 lines" }
        };

        cacheManipulator.GetMBSpreadsByDate(request.EventDate).Returns(cachedMBSpreads);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert - SaveEventLog should not be called at all when there are no changes
        await repository.DidNotReceive().SaveEventLog(Arg.Any<SaveEditEventLogsCriteria>());

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldFormatSpreadAndMaxbetValues_WhenGeneratingLogData()
    {
        // Arrange
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var session = Substitute.For<INapSession>();
        var cacheManipulator = Substitute.For<CacheManipulator>();

        session.UserId.Returns(789);
        session.UserName.Returns("testuser");

        var handler = new UpdateMaxbetHandler(repository, session, cacheManipulator);

        var events = new List<UpdateEvents>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 100000, SpreadHdp = 0.5m, SpreadOu = 0.25m }
        };

        var request = new UpdateEventsRequest
        {
            Events = events,
            EventDate = new DateTime(2024, 1, 15)
        };

        var cachedMBSpreads = new List<EventMBSpread>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 50000, SpreadHdp = 0.3m, SpreadOu = 0.15m }
        };

        cacheManipulator.GetMBSpreadsByDate(request.EventDate).Returns(cachedMBSpreads);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).SaveEventLog(
            Arg.Is<SaveEditEventLogsCriteria>(c =>
                !string.IsNullOrEmpty(c.Xml) &&
                c.Xml.Contains("100K") && // 100000/1000 = 100K (rounded)
                c.Xml.Contains("50%") &&  // 0.5 * 100 = 50% (rounded)
                c.Xml.Contains("25%"))); // 0.25 * 100 = 25% (rounded)

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldHandleNullValues_WhenComparingData()
    {
        // Arrange
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var session = Substitute.For<INapSession>();
        var cacheManipulator = Substitute.For<CacheManipulator>();

        session.UserId.Returns(789);
        session.UserName.Returns("testuser");

        var handler = new UpdateMaxbetHandler(repository, session, cacheManipulator);

        var events = new List<UpdateEvents>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 100000, SpreadHdp = null, NoOfHDP = "2 lines" }
        };

        var request = new UpdateEventsRequest
        {
            Events = events,
            EventDate = new DateTime(2024, 1, 15)
        };

        var cachedMBSpreads = new List<EventMBSpread>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = null, SpreadHdp = 0.3m, NoOfHDP = null }
        };

        cacheManipulator.GetMBSpreadsByDate(request.EventDate).Returns(cachedMBSpreads);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).SaveEventLog(
            Arg.Is<SaveEditEventLogsCriteria>(c =>
                !string.IsNullOrEmpty(c.Xml) &&
                c.Xml.Contains("MB") &&
                c.Xml.Contains("SpreadHDP") &&
                c.Xml.Contains("No.HDP")));

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldSkipEvents_WhenNotInCache()
    {
        // Arrange
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var session = Substitute.For<INapSession>();
        var cacheManipulator = Substitute.For<CacheManipulator>();

        session.UserId.Returns(789);
        session.UserName.Returns("testuser");

        var handler = new UpdateMaxbetHandler(repository, session, cacheManipulator);

        var events = new List<UpdateEvents>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 100000, SpreadHdp = 0.5m },
            new() { InfoSiteMatchId = 999, MaxbetHdp = 200000, SpreadHdp = 1.0m } // This ID not in cache
        };

        var request = new UpdateEventsRequest
        {
            Events = events,
            EventDate = new DateTime(2024, 1, 15)
        };

        var cachedMBSpreads = new List<EventMBSpread>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 50000, SpreadHdp = 0.3m }
            // InfoSiteMatchId 999 is missing from cache
        };

        cacheManipulator.GetMBSpreadsByDate(request.EventDate).Returns(cachedMBSpreads);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).SaveEventLog(
            Arg.Is<SaveEditEventLogsCriteria>(c =>
                !string.IsNullOrEmpty(c.Xml) &&
                c.Xml.Contains("MB") &&
                c.Xml.Contains("SpreadHDP")));

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldRoundDecimalValues_WhenFormattingMaxbet()
    {
        // Arrange
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var session = Substitute.For<INapSession>();
        var cacheManipulator = Substitute.For<CacheManipulator>();

        session.UserId.Returns(789);
        session.UserName.Returns("testuser");

        var handler = new UpdateMaxbetHandler(repository, session, cacheManipulator);

        var events = new List<UpdateEvents>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 20000 } // 20000/1000 = 20.0K should become 20K
        };

        var request = new UpdateEventsRequest
        {
            Events = events,
            EventDate = new DateTime(2024, 1, 15)
        };

        var cachedMBSpreads = new List<EventMBSpread>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 21000 } // 21000/1000 = 21.0K should become 21K
        };

        cacheManipulator.GetMBSpreadsByDate(request.EventDate).Returns(cachedMBSpreads);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).SaveEventLog(
            Arg.Is<SaveEditEventLogsCriteria>(c =>
                !string.IsNullOrEmpty(c.Xml) &&
                c.Xml.Contains("20K") && // Should be 20K, not 20.0K
                c.Xml.Contains("21K"))); // Should be 21K, not 21.0K

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldMergeCacheData_WhenUpdatingExistingAndAddingNewEvents()
    {
        // Arrange
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var session = Substitute.For<INapSession>();
        var cacheManipulator = Substitute.For<CacheManipulator>();

        session.UserId.Returns(789);
        session.UserName.Returns("testuser");

        var handler = new UpdateMaxbetHandler(repository, session, cacheManipulator);

        var events = new List<UpdateEvents>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 100, SpreadHdp = 0.5m }, // Existing event - should be updated
            new() { InfoSiteMatchId = 999, MaxbetHdp = 300, SpreadHdp = 1.0m }  // New event - should be added
        };

        var request = new UpdateEventsRequest
        {
            Events = events,
            EventDate = new DateTime(2024, 1, 15)
        };

        var cachedMBSpreads = new List<EventMBSpread>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 50, SpreadHdp = 0.3m, IsReferenceLeagueManagement = true },
            new() { InfoSiteMatchId = 456, MaxbetHdp = 150, SpreadOu = 0.8m, IsReferenceLeagueManagement = true } // Should remain unchanged
        };

        cacheManipulator.GetMBSpreadsByDate(request.EventDate).Returns(cachedMBSpreads);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await cacheManipulator.Received(1).SaveMBSpreadsByDate(request.EventDate,
            Arg.Is<List<EventMBSpread>>(cachedData =>
                cachedData.Count == 3 && // Should contain all 3 events
                cachedData.Any(c => c.InfoSiteMatchId == 123 && c.MaxbetHdp == 100 && !c.IsReferenceLeagueManagement) && // Updated existing event
                cachedData.Any(c => c.InfoSiteMatchId == 999 && c.MaxbetHdp == 300 && !c.IsReferenceLeagueManagement) && // Added new event
                cachedData.Any(c => c.InfoSiteMatchId == 456 && c.MaxbetHdp == 150 && c.IsReferenceLeagueManagement))); // Unchanged existing event

        Assert.Equal(Unit.Value, result);
    }
}