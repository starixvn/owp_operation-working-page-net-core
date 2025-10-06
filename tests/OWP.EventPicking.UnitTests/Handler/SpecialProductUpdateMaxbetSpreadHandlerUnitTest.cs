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
        await cacheManipulator.Received(1).SaveMBSpreadsByDate(request.EventDate, Arg.Any<List<EventMBSpread>>());

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
            new() { InfoSiteMatchId = 123, MaxbetHdp = 100, SpreadHdp = 0.5m, NoOfHDP = "2 lines" }
        };

        var request = new UpdateEventsRequest
        {
            Events = events,
            EventDate = new DateTime(2024, 1, 15)
        };

        var cachedMBSpreads = new List<EventMBSpread>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 50, SpreadHdp = 0.3m, NoOfHDP = "1 line" }
        };

        cacheManipulator.GetMBSpreadsByDate(request.EventDate).Returns(cachedMBSpreads);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).SaveEventLog(
            Arg.Is<SaveEditEventLogsCriteria>(c => 
                !string.IsNullOrEmpty(c.Xml) && 
                c.Xml.Contains("MaxbetHdp") && 
                c.Xml.Contains("SpreadHdp") &&
                c.Xml.Contains("NoOfHDP")));

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
            new() { InfoSiteMatchId = 123, MaxbetHdp = 100, SpreadHdp = 0.5m, NoOfHDP = "2 lines" }
        };

        var request = new UpdateEventsRequest
        {
            Events = events,
            EventDate = new DateTime(2024, 1, 15)
        };

        var cachedMBSpreads = new List<EventMBSpread>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 100, SpreadHdp = 0.5m, NoOfHDP = "2 lines" }
        };

        cacheManipulator.GetMBSpreadsByDate(request.EventDate).Returns(cachedMBSpreads);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).SaveEventLog(
            Arg.Is<SaveEditEventLogsCriteria>(c => 
                string.IsNullOrEmpty(c.Xml) || !c.Xml.Contains("MaxbetHdp")));

        Assert.Equal(Unit.Value, result);
    }
}