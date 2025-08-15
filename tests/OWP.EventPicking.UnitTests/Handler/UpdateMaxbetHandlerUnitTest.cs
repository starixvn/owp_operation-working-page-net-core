using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fanex.NAP.Client;
using MediatR;
using Newtonsoft.Json;
using NSubstitute;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Models;
using Xunit;

namespace OWP.EventPicking.UnitTests.Handler;

public class UpdateMaxbetHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldSerializeEvents_AndCallRepository_AndReturnUnit()
    {
        // Arrange
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(456);

        var handler = new UpdateMaxbetHandler(repository, session);

        var events = new List<UpdateEvents>
        {
            new() { InfoSiteMatchId = 123, MaxbetHdp = 1000, SpreadHdp = 0.05m, NoOfHDP = "1" },
            new() { InfoSiteMatchId = 456, MaxbetHdp = 2000, SpreadOu = 0.10m, NoOfHDP = "2" }
        };

        var request = new UpdateEventsRequest
        {
            Events = events
        };

        var expectedJson = JsonConvert.SerializeObject(events);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).UpdateEvent(
            Arg.Is<UpdateMaxbetSpreadCriteria>(c =>
                c.ActorId == 456 &&
                c.TargetJson == expectedJson));

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithEmptyEvents_ShouldCallRepository_AndReturnUnit()
    {
        // Arrange
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(789);

        var handler = new UpdateMaxbetHandler(repository, session);

        var events = new List<UpdateEvents>();

        var request = new UpdateEventsRequest
        {
            Events = events
        };

        var expectedJson = JsonConvert.SerializeObject(events);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).UpdateEvent(
            Arg.Is<UpdateMaxbetSpreadCriteria>(c =>
                c.ActorId == 789 &&
                c.TargetJson == expectedJson));

        Assert.Equal(Unit.Value, result);
    }
}