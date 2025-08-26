using System.Threading;
using System.Threading.Tasks;
using Fanex.NAP.Client;
using MediatR;
using NSubstitute;
using OWP.CQRS;
using OWP.NewLeagueSetting.Command;
using OWP.NewLeagueSetting.Criteria;
using OWP.NewLeagueSetting.Data;
using Xunit;

namespace OWP.NewLeagueSetting.UnitTests;

public class RemoveLeagueSettingHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldCreateRemoveSettingCriteria_AndCallRepository_AndReturnUnit()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        session.UserId.Returns(123);

        var handler = new RemoveLeagueSettingHandler(repository, session);

        var request = new RemoveLeagueSettingRequest(456);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<RemoveSettingCriteria>(c =>
                c.LeagueSettingId == 456 &&
                c.ActorId == 123));
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithDifferentLeagueSettingId_ShouldCreateCorrectCriteria()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        session.UserId.Returns(789);

        var handler = new RemoveLeagueSettingHandler(repository, session);

        var request = new RemoveLeagueSettingRequest(999);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<RemoveSettingCriteria>(c =>
                c.LeagueSettingId == 999 &&
                c.ActorId == 789));
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithZeroUserId_ShouldStillProcessRequest()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        session.UserId.Returns(0);

        var handler = new RemoveLeagueSettingHandler(repository, session);

        var request = new RemoveLeagueSettingRequest(456);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<RemoveSettingCriteria>(c =>
                c.LeagueSettingId == 456 &&
                c.ActorId == 0));
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithNegativeUserId_ShouldStillProcessRequest()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        session.UserId.Returns(-1);

        var handler = new RemoveLeagueSettingHandler(repository, session);

        var request = new RemoveLeagueSettingRequest(456);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<RemoveSettingCriteria>(c =>
                c.LeagueSettingId == 456 &&
                c.ActorId == -1));
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithZeroLeagueSettingId_ShouldStillProcessRequest()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        session.UserId.Returns(123);

        var handler = new RemoveLeagueSettingHandler(repository, session);

        var request = new RemoveLeagueSettingRequest(0);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<RemoveSettingCriteria>(c =>
                c.LeagueSettingId == 0 &&
                c.ActorId == 123));
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithNegativeLeagueSettingId_ShouldStillProcessRequest()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        session.UserId.Returns(123);

        var handler = new RemoveLeagueSettingHandler(repository, session);

        var request = new RemoveLeagueSettingRequest(-1);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<RemoveSettingCriteria>(c =>
                c.LeagueSettingId == -1 &&
                c.ActorId == 123));
        Assert.Equal(Unit.Value, result);
    }
}
