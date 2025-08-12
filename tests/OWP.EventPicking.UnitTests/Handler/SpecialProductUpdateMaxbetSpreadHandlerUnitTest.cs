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
using Xunit;

namespace OWP.EventPicking.UnitTests.Handler;

public class SpecialProductUpdateMaxbetSpreadHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldSerializeEvents_AndCallRepository_AndReturnUnit()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(789);

        var handler = new SpecialProductUpdateMaxbetSpreadHandler(repository, session);

        var events = new List<SpecialProductMaxbetSpread>
        {
            new() { ParentId = 123, MaxbetHdp = 100, SpreadHdp = 0.5m },
            new() { ParentId = 456, MaxbetHdp = 200, SpreadOu = 1.0m }
        };

        var request = new SpecialProductUpdateMaxbetSpreadRequest
        {
            Events = events
        };

        var expectedJson = JsonConvert.SerializeObject(events);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQuery(
            Arg.Is<SpecialProductUpdateMaxbetSpreadCriteria>(c =>
                c.ActorId == 789 &&
                c.TargetJson == expectedJson));

        Assert.Equal(Unit.Value, result);
    }
}