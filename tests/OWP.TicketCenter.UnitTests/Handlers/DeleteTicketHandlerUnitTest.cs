using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NSubstitute;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Middleware.Models;
using Xunit;

namespace OWP.TicketCenter.UnitTests.Handlers;

public class DeleteTicketHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldCallRepositoryDeleteTicket_AndReturnTrue()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new DeleteTicketHandler(repository);

        var request = new DeleteTicketRequest
        {
            ActorId = 123,
            TicketId = 456
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).DeleteTicket(
            Arg.Is<DeleteTicketCriteria>(c =>
                c.UserId == 123 &&
                c.TicketId == 456 &&
                c.EventDate.Date == DateTime.Now.Date));

        Assert.True(result);
    }

    [Fact]
    public async Task Handle_WithDifferentActorAndTicket_ShouldCallRepositoryWithCorrectValues()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new DeleteTicketHandler(repository);

        var request = new DeleteTicketRequest
        {
            ActorId = 789,
            TicketId = 999
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).DeleteTicket(
            Arg.Is<DeleteTicketCriteria>(c =>
                c.UserId == 789 &&
                c.TicketId == 999 &&
                c.EventDate.Date == DateTime.Now.Date));

        Assert.True(result);
    }
}