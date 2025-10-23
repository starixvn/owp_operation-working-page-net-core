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
    public async Task Handle_ShouldGenerateCrossJoinOfParentAndMinorIds()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(123);

        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var childMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();

        var mainEvents = new List<EventPickingModel>
        {
            new() { InfoSiteMatchId = 10, MaxbetHdp = 5000 },
            new() { InfoSiteMatchId = 20, MaxbetHdp = 10000 },
            new() { InfoSiteMatchId = 30, MaxbetHdp = 15000 }
        };

        repository.QueryMainMatch("10,20,30").Returns(mainEvents);

        var handler = new AddSpecialProductHandler(session, repository, childMatchMaxBet);

        var addInfo = new AddSpecialProduct
        {
            ParentId = "10,20,30",
            MinorBettypeId = "101,102"
        };
        var request = new AddSpecialProductRequest(addInfo);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryMainMatch("10,20,30");
        
        await childMatchMaxBet.Received(1).BuildMBForAddSpecialProduct(
            Arg.Is<List<ChildMatchModel>>(list =>
                list.Count == 6 &&
                list.Any(x => x.ParentMatchId == 10 && x.MinorBettypeId == 101) &&
                list.Any(x => x.ParentMatchId == 10 && x.MinorBettypeId == 102) &&
                list.Any(x => x.ParentMatchId == 20 && x.MinorBettypeId == 101) &&
                list.Any(x => x.ParentMatchId == 20 && x.MinorBettypeId == 102) &&
                list.Any(x => x.ParentMatchId == 30 && x.MinorBettypeId == 101) &&
                list.Any(x => x.ParentMatchId == 30 && x.MinorBettypeId == 102)),
            Arg.Is<List<EventPickingModel>>(m => m == mainEvents));

        await repository.Received(1).ExecuteNonQuery(
            Arg.Is<AddSpecialProductCriteria>(c =>
                c.ActorId == 123 &&
                c.ChildMatches != null));

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldSetAllNullablePropertiesToNull()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(456);

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
            Arg.Is<List<ChildMatchModel>>(list =>
                list.Count == 1 &&
                list[0].ParentMatchId == 100 &&
                list[0].MinorBettypeId == 50 &&
                list[0].MaxbetHDP == null &&
                list[0].MaxbetOU == null &&
                list[0].SpreadHDP == null &&
                list[0].SpreadOU == null &&
                list[0].NoOfHDP == null),
            Arg.Any<List<EventPickingModel>>());
    }

    [Fact]
    public async Task Handle_SingleParentMultipleMinorIds_ShouldGenerate3Products()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
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
            Arg.Is<List<ChildMatchModel>>(list =>
                list.Count == 3 &&
                list.All(x => x.ParentMatchId == 1) &&
                list.Any(x => x.MinorBettypeId == 10) &&
                list.Any(x => x.MinorBettypeId == 20) &&
                list.Any(x => x.MinorBettypeId == 30)),
            Arg.Any<List<EventPickingModel>>());
    }

    [Fact]
    public async Task Handle_MultipleParentsSingleMinorId_ShouldGenerate3Products()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var childMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();

        repository.QueryMainMatch(Arg.Any<string>()).Returns(new List<EventPickingModel>
        {
            new() { InfoSiteMatchId = 100, MaxbetHdp = 5000 },
            new() { InfoSiteMatchId = 200, MaxbetHdp = 10000 },
            new() { InfoSiteMatchId = 300, MaxbetHdp = 15000 }
        });

        var handler = new AddSpecialProductHandler(session, repository, childMatchMaxBet);

        var addInfo = new AddSpecialProduct
        {
            ParentId = "100,200,300",
            MinorBettypeId = "5"
        };
        var request = new AddSpecialProductRequest(addInfo);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        await childMatchMaxBet.Received(1).BuildMBForAddSpecialProduct(
            Arg.Is<List<ChildMatchModel>>(list =>
                list.Count == 3 &&
                list.All(x => x.MinorBettypeId == 5) &&
                list.Any(x => x.ParentMatchId == 100) &&
                list.Any(x => x.ParentMatchId == 200) &&
                list.Any(x => x.ParentMatchId == 300)),
            Arg.Any<List<EventPickingModel>>());
    }

    [Fact]
    public async Task Handle_ShouldTrimWhitespaceFromIds()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var childMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();

        repository.QueryMainMatch(Arg.Any<string>()).Returns(new List<EventPickingModel>
        {
            new() { InfoSiteMatchId = 1, MaxbetHdp = 5000 },
            new() { InfoSiteMatchId = 2, MaxbetHdp = 10000 }
        });

        var handler = new AddSpecialProductHandler(session, repository, childMatchMaxBet);

        var addInfo = new AddSpecialProduct
        {
            ParentId = " 1 , 2 ",
            MinorBettypeId = " 10 , 20 "
        };
        var request = new AddSpecialProductRequest(addInfo);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        await childMatchMaxBet.Received(1).BuildMBForAddSpecialProduct(
            Arg.Is<List<ChildMatchModel>>(list =>
                list.Count == 4 &&
                list.Any(x => x.ParentMatchId == 1 && x.MinorBettypeId == 10) &&
                list.Any(x => x.ParentMatchId == 1 && x.MinorBettypeId == 20) &&
                list.Any(x => x.ParentMatchId == 2 && x.MinorBettypeId == 10) &&
                list.Any(x => x.ParentMatchId == 2 && x.MinorBettypeId == 20)),
            Arg.Any<List<EventPickingModel>>());
    }

    [Fact]
    public async Task Handle_ShouldRemoveDuplicateIds()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var childMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();

        repository.QueryMainMatch(Arg.Any<string>()).Returns(new List<EventPickingModel>
        {
            new() { InfoSiteMatchId = 1, MaxbetHdp = 5000 },
            new() { InfoSiteMatchId = 2, MaxbetHdp = 10000 }
        });

        var handler = new AddSpecialProductHandler(session, repository, childMatchMaxBet);

        var addInfo = new AddSpecialProduct
        {
            ParentId = "1,1,2,2,1",
            MinorBettypeId = "10,20,10,20"
        };
        var request = new AddSpecialProductRequest(addInfo);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert - Should only generate 4 products (2 unique parents × 2 unique minors)
        await childMatchMaxBet.Received(1).BuildMBForAddSpecialProduct(
            Arg.Is<List<ChildMatchModel>>(list =>
                list.Count == 4 &&
                list.Any(x => x.ParentMatchId == 1 && x.MinorBettypeId == 10) &&
                list.Any(x => x.ParentMatchId == 1 && x.MinorBettypeId == 20) &&
                list.Any(x => x.ParentMatchId == 2 && x.MinorBettypeId == 10) &&
                list.Any(x => x.ParentMatchId == 2 && x.MinorBettypeId == 20)),
            Arg.Any<List<EventPickingModel>>());
    }
}