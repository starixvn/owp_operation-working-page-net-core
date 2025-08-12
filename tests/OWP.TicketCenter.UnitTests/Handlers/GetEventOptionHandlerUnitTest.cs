using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NSubstitute;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;
using Xunit;

namespace OWP.TicketCenter.UnitTests.Handlers;

public class GetEventOptionHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldGroupEventsByLeague_AndReturnEventCheckingOptionInfoSender()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new GetEventOptionHandler(repository);

        var request = new GetTicketEventOptionRequest
        {
            Keyword = "test keyword"
        };

        var eventInfo = new List<EventCheckingOption>
        {
            new()
            {
                InfoSiteMatchId = 1,
                OwpLeagueId = 100,
                LeagueName = "Test League 1",
                HomeName = "Team A",
                HomeId = 1,
                AwayName = "Team B",
                AwayId = 2,
                EventDateTimeGmt8 = System.DateTime.Now,
                IsLeague = true,
                HasAssignee = true,
                RecipientId = 123,
                UserName = "user1",
                NickName = "User One"
            },
            new()
            {
                InfoSiteMatchId = 2,
                OwpLeagueId = 100,
                LeagueName = "Test League 1",
                HomeName = "Team C",
                HomeId = 3,
                AwayName = "Team D",
                AwayId = 4,
                EventDateTimeGmt8 = System.DateTime.Now.AddHours(1),
                IsLeague = true,
                HasAssignee = true,
                RecipientId = 456,
                UserName = "user2",
                NickName = "User Two"
            },
            new()
            {
                InfoSiteMatchId = 3,
                OwpLeagueId = 200,
                LeagueName = "Test League 2",
                HomeName = "Team E",
                HomeId = 5,
                AwayName = "Team F",
                AwayId = 6,
                EventDateTimeGmt8 = System.DateTime.Now.AddHours(2),
                IsLeague = false,
                HasAssignee = false,
                RecipientId = null,
                UserName = null,
                NickName = null
            }
        };

        repository.GetEventOptionInfo(request.Keyword).Returns(eventInfo);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).GetEventOptionInfo(request.Keyword);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count); // 2 different leagues

        var firstLeague = result.First(r => r.EventInfoViewModel.First().LeagueName == "Test League 1");
        Assert.Equal(2, firstLeague.EventInfoViewModel.Count);
        Assert.Equal(2, firstLeague.UserViewModel.Count);

        var secondLeague = result.First(r => r.EventInfoViewModel.First().LeagueName == "Test League 2");
        Assert.Single(secondLeague.EventInfoViewModel);
        Assert.Empty(secondLeague.UserViewModel); // No assignees
    }

    [Fact]
    public async Task Handle_WithEmptyResult_ShouldReturnEmptyList()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new GetEventOptionHandler(repository);

        var request = new GetTicketEventOptionRequest
        {
            Keyword = "nonexistent"
        };

        repository.GetEventOptionInfo(request.Keyword).Returns(new List<EventCheckingOption>());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).GetEventOptionInfo(request.Keyword);
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}