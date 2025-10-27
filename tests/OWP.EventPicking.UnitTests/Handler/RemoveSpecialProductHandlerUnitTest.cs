using System.Threading;
using System.Threading.Tasks;
using Fanex.NAP.Client;
using MediatR;
using NSubstitute;
using OWP.CQRS;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Models;
using Xunit;

namespace OWP.EventPicking.UnitTests.Handler;

public class RemoveSpecialProductHandlerUnitTest
{
    [Fact]
    public async Task Handle_SingleParentMultipleMinorIds_ShouldExecuteAndLog()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(456);
        session.UserName.Returns("TestUser");

        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var handler = new RemoveSpecialProductHandler(session, repository);

        var removeInfo = new RemoveSpecialProduct
        {
            ParentId = "50",
            MinorBettypeId = "201,202,203"
        };
        var request = new RemoveSpecialProductRequest(removeInfo);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQuery(
            Arg.Is<RemoveSpecialProductCriteria>(c =>
                c.ActorId == 456 &&
                c.ParentId == "50" &&
                c.MinorBettypeId == "201,202,203"));

        await repository.Received(1).SaveEventLog(Arg.Any<SaveEditEventLogsCriteria>());

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_SingleParentSingleMinorId_ShouldExecuteAndLog()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(789);
        session.UserName.Returns("TestUser");

        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var handler = new RemoveSpecialProductHandler(session, repository);

        var removeInfo = new RemoveSpecialProduct
        {
            ParentId = "100",
            MinorBettypeId = "5"
        };
        var request = new RemoveSpecialProductRequest(removeInfo);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQuery(
            Arg.Is<RemoveSpecialProductCriteria>(c =>
                c.ActorId == 789 &&
                c.ParentId == "100" &&
                c.MinorBettypeId == "5"));

        await repository.Received(1).SaveEventLog(Arg.Any<SaveEditEventLogsCriteria>());

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldPassCorrectActorIdFromSession()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(999);
        session.UserName.Returns("AdminUser");

        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var handler = new RemoveSpecialProductHandler(session, repository);

        var removeInfo = new RemoveSpecialProduct
        {
            ParentId = "10",
            MinorBettypeId = "20"
        };
        var request = new RemoveSpecialProductRequest(removeInfo);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQuery(
            Arg.Is<RemoveSpecialProductCriteria>(c => c.ActorId == 999));
    }

    [Fact]
    public async Task Handle_ShouldPreserveParentAndMinorIdStrings()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(123);
        session.UserName.Returns("TestUser");

        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var handler = new RemoveSpecialProductHandler(session, repository);

        var removeInfo = new RemoveSpecialProduct
        {
            ParentId = "777",
            MinorBettypeId = "888,999"
        };
        var request = new RemoveSpecialProductRequest(removeInfo);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQuery(
            Arg.Is<RemoveSpecialProductCriteria>(c =>
                c.ParentId == "777" &&
                c.MinorBettypeId == "888,999"));
    }
}