using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NSubstitute;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using OWP.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Handler;

public class InsertOrDeleteNoOfferMatchHandlerUnitTest
{
    [Fact]
    public async Task Handle_SingleSelected_ShouldCallRepository_AndNotUpdateCounter()
    {
        // Arrange
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var accessCounter = Substitute.For<AccessCounter>();

        var handler = new InsertOrDeleteNoOfferMatchHandler(repository, accessCounter);

        var request = new InsertOrUpdateNoOfferMatchRequest
        {
            NoOfferMatch = new List<NoOfferMatchInfo>
            {
                new() { InfoSiteMatchId = 123 }
            },
            IsMultiSelected = false
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).InsertOrUpdateNoOfferMatch(request);
        await accessCounter.DidNotReceive().UpdateCounter(Arg.Any<string>());

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_MultiSelected_ShouldCallRepository_AndUpdateCounter()
    {
        // Arrange
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var accessCounter = Substitute.For<AccessCounter>();

        var handler = new InsertOrDeleteNoOfferMatchHandler(repository, accessCounter);

        var request = new InsertOrUpdateNoOfferMatchRequest
        {
            NoOfferMatch = new List<NoOfferMatchInfo>
            {
                new() { InfoSiteMatchId = 123 },
                new() { InfoSiteMatchId = 456 },
                new() { InfoSiteMatchId = 789 }
            },
            IsMultiSelected = true
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).InsertOrUpdateNoOfferMatch(request);
        await accessCounter.Received(1).UpdateCounter("EventPicking Multiple NoOffer");

        Assert.Equal(Unit.Value, result);
    }
}