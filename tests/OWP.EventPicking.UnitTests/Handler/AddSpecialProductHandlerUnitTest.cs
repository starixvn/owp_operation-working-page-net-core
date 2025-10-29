using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fanex.NAP.Client;
using MediatR;
using NSubstitute;
using OWP.CQRS;
using OWP.EventPicking;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Handler;

public class AddSpecialProductHandlerUnitTest
{
    [Fact]
    public async Task Handle_SingleParentMultipleMinorIds_ShouldGenerateCorrectProducts()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(123);
        session.UserName.Returns("TestUser");

        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var childMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();

        var mainEvents = new List<EventPickingModel>
        {
            new() { InfoSiteMatchId = 10, MaxbetHdp = 5000 }
        };

        repository.QueryMainMatch("10").Returns(mainEvents);

        var handler = new AddSpecialProductHandler(session, repository, childMatchMaxBet);

        var addInfo = new AddSpecialProduct
        {
            ParentId = "10",
            MinorBettypeId = "101,102"
        };
        var request = new AddSpecialProductRequest(addInfo);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryMainMatch("10");

        await childMatchMaxBet.Received(1).BuildMBForAddSpecialProduct(
            Arg.Is<List<SpecialProductMaxbetSpread>>(list =>
                list.Count == 2 &&
                list.Any(x => x.ParentId == 10 && x.MinorBettypeId == 101) &&
                list.Any(x => x.ParentId == 10 && x.MinorBettypeId == 102)),
            Arg.Is<List<EventPickingModel>>(m => m == mainEvents));

        await repository.Received(1).ExecuteNonQuery(
            Arg.Is<AddSpecialProductCriteria>(c =>
                c.ActorId == 123 &&
                c.ChildMatches != null));

        await repository.Received(1).SaveEventLog(Arg.Any<SaveEditEventLogsCriteria>());

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldSetAllNullablePropertiesToNull()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(456);
        session.UserName.Returns("TestUser");

        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var childMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();

        var mainEvents = new List<EventPickingModel>
        {
            new() { InfoSiteMatchId = 100, MaxbetHdp = 3000 }
        };

        repository.QueryMainMatch("100").Returns(mainEvents);

        var handler = new AddSpecialProductHandler(session, repository, childMatchMaxBet);

        var addInfo = new AddSpecialProduct
        {
            ParentId = "100",
            MinorBettypeId = "50"
        };
        var request = new AddSpecialProductRequest(addInfo);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        await childMatchMaxBet.Received(1).BuildMBForAddSpecialProduct(
            Arg.Is<List<SpecialProductMaxbetSpread>>(list =>
                list.Count == 1 &&
                list[0].ParentId == 100 &&
                list[0].MinorBettypeId == 50 &&
                list[0].MaxbetHdp == null &&
                list[0].MaxbetOu == null &&
                list[0].SpreadHdp == null &&
                list[0].SpreadOu == null &&
                list[0].NoOfHDP == null),
            Arg.Any<List<EventPickingModel>>());
    }

    [Fact]
    public async Task Handle_SingleParentMultipleMinorIds_ShouldGenerate3Products()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserName.Returns("TestUser");

        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var childMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();

        repository.QueryMainMatch(Arg.Any<string>()).Returns(new List<EventPickingModel>
        {
            new() { InfoSiteMatchId = 1, MaxbetHdp = 5000 }
        });

        var handler = new AddSpecialProductHandler(session, repository, childMatchMaxBet);

        var addInfo = new AddSpecialProduct
        {
            ParentId = "1",
            MinorBettypeId = "10,20,30"
        };
        var request = new AddSpecialProductRequest(addInfo);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        await childMatchMaxBet.Received(1).BuildMBForAddSpecialProduct(
            Arg.Is<List<SpecialProductMaxbetSpread>>(list =>
                list.Count == 3 &&
                list.All(x => x.ParentId == 1) &&
                list.Any(x => x.MinorBettypeId == 10) &&
                list.Any(x => x.MinorBettypeId == 20) &&
                list.Any(x => x.MinorBettypeId == 30)),
            Arg.Any<List<EventPickingModel>>());
    }

    [Fact]
    public async Task Handle_SingleParentSingleMinorId_ShouldGenerateOneProduct()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserName.Returns("TestUser");

        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var childMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();

        repository.QueryMainMatch(Arg.Any<string>()).Returns(new List<EventPickingModel>
        {
            new() { InfoSiteMatchId = 100, MaxbetHdp = 5000 }
        });

        var handler = new AddSpecialProductHandler(session, repository, childMatchMaxBet);

        var addInfo = new AddSpecialProduct
        {
            ParentId = "100",
            MinorBettypeId = "5"
        };
        var request = new AddSpecialProductRequest(addInfo);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        await childMatchMaxBet.Received(1).BuildMBForAddSpecialProduct(
            Arg.Is<List<SpecialProductMaxbetSpread>>(list =>
                list.Count == 1 &&
                list[0].ParentId == 100 &&
                list[0].MinorBettypeId == 5),
            Arg.Any<List<EventPickingModel>>());
    }

    [Fact]
    public async Task Handle_ShouldTrimWhitespaceFromIds()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserName.Returns("TestUser");

        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var childMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();

        repository.QueryMainMatch(Arg.Any<string>()).Returns(new List<EventPickingModel>
        {
            new() { InfoSiteMatchId = 1, MaxbetHdp = 5000 }
        });

        var handler = new AddSpecialProductHandler(session, repository, childMatchMaxBet);

        var addInfo = new AddSpecialProduct
        {
            ParentId = " 1 ",
            MinorBettypeId = " 10 , 20 "
        };
        var request = new AddSpecialProductRequest(addInfo);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        await childMatchMaxBet.Received(1).BuildMBForAddSpecialProduct(
            Arg.Is<List<SpecialProductMaxbetSpread>>(list =>
                list.Count == 2 &&
                list.Any(x => x.ParentId == 1 && x.MinorBettypeId == 10) &&
                list.Any(x => x.ParentId == 1 && x.MinorBettypeId == 20)),
            Arg.Any<List<EventPickingModel>>());
    }

    [Fact]
    public async Task Handle_ShouldRemoveDuplicateMinorIds()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserName.Returns("TestUser");

        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var childMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();

        repository.QueryMainMatch(Arg.Any<string>()).Returns(new List<EventPickingModel>
        {
            new() { InfoSiteMatchId = 1, MaxbetHdp = 5000 }
        });

        var handler = new AddSpecialProductHandler(session, repository, childMatchMaxBet);

        var addInfo = new AddSpecialProduct
        {
            ParentId = "1",
            MinorBettypeId = "10,20,10,20"
        };
        var request = new AddSpecialProductRequest(addInfo);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert - Should only generate 2 products (1 parent × 2 unique minors)
        await childMatchMaxBet.Received(1).BuildMBForAddSpecialProduct(
            Arg.Is<List<SpecialProductMaxbetSpread>>(list =>
                list.Count == 2 &&
                list.Any(x => x.ParentId == 1 && x.MinorBettypeId == 10) &&
                list.Any(x => x.ParentId == 1 && x.MinorBettypeId == 20)),
            Arg.Any<List<EventPickingModel>>());
    }

    [Fact]
    public async Task Handle_EmptyMinorBettypeId_ShouldReturnEarlyWithoutProcessing()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserName.Returns("TestUser");

        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var childMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();

        var handler = new AddSpecialProductHandler(session, repository, childMatchMaxBet);

        var addInfo = new AddSpecialProduct
        {
            ParentId = "1",
            MinorBettypeId = ""
        };
        var request = new AddSpecialProductRequest(addInfo);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.DidNotReceive().QueryMainMatch(Arg.Any<string>());
        await childMatchMaxBet.DidNotReceive().BuildMBForAddSpecialProduct(
            Arg.Any<List<SpecialProductMaxbetSpread>>(),
            Arg.Any<List<EventPickingModel>>());
        await repository.DidNotReceive().ExecuteNonQuery(Arg.Any<AddSpecialProductCriteria>());

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_EmptyParentId_ShouldReturnEarlyWithoutProcessing()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserName.Returns("TestUser");

        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var childMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();

        var handler = new AddSpecialProductHandler(session, repository, childMatchMaxBet);

        var addInfo = new AddSpecialProduct
        {
            ParentId = "",
            MinorBettypeId = "10"
        };
        var request = new AddSpecialProductRequest(addInfo);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.DidNotReceive().QueryMainMatch(Arg.Any<string>());
        await childMatchMaxBet.DidNotReceive().BuildMBForAddSpecialProduct(
            Arg.Any<List<SpecialProductMaxbetSpread>>(),
            Arg.Any<List<EventPickingModel>>());
        await repository.DidNotReceive().ExecuteNonQuery(Arg.Any<AddSpecialProductCriteria>());

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldCallSaveEventLogWithCorrectNumberOfLogs()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(999);
        session.UserName.Returns("TestUser");

        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var childMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();

        repository.QueryMainMatch(Arg.Any<string>()).Returns(new List<EventPickingModel>
        {
            new() { InfoSiteMatchId = 50, MaxbetHdp = 5000 }
        });

        var handler = new AddSpecialProductHandler(session, repository, childMatchMaxBet);

        var addInfo = new AddSpecialProduct
        {
            ParentId = "50",
            MinorBettypeId = "1,2,3"
        };
        var request = new AddSpecialProductRequest(addInfo);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert - Should log for each minor bettype ID
        await repository.Received(1).SaveEventLog(Arg.Any<SaveEditEventLogsCriteria>());
    }

    [Fact]
    public async Task Handle_ShouldPassCorrectMainEventToMaxbetBuilder()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserName.Returns("TestUser");

        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var childMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();

        var expectedMainEvents = new List<EventPickingModel>
        {
            new() { InfoSiteMatchId = 777, MaxbetHdp = 12345 }
        };

        repository.QueryMainMatch("777").Returns(expectedMainEvents);

        var handler = new AddSpecialProductHandler(session, repository, childMatchMaxBet);

        var addInfo = new AddSpecialProduct
        {
            ParentId = "777",
            MinorBettypeId = "99"
        };
        var request = new AddSpecialProductRequest(addInfo);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryMainMatch("777");

        await childMatchMaxBet.Received(1).BuildMBForAddSpecialProduct(
            Arg.Any<List<SpecialProductMaxbetSpread>>(),
            Arg.Is<List<EventPickingModel>>(m =>
                m.Count == 1 &&
                m[0].InfoSiteMatchId == 777 &&
                m[0].MaxbetHdp == 12345));
    }
}