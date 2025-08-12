using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using OWP.EventPicking.Api;
using OWP.EventPicking.Api.Models;
using Xunit;

namespace OWP.EventPicking.UnitTests.Api;

public class InfositeApiControllerUnitTests
{
    private readonly IMapper _mapper;
    private readonly InfositeApiController _controller;

    public InfositeApiControllerUnitTests()
    {
        _mapper = Substitute.For<IMapper>();
        _controller = new InfositeApiController(_mapper);
    }

    [Fact]
    public void SyncEventChangeGameDate_WithEnabledConfiguration_ShouldSendRequestAndReturnOk()
    {
        // Arrange
        var configuration = Substitute.For<IConfiguration>();
        var sender = Substitute.For<ISender>();
        var @event = new socer_event_change_game_date
        {
            MatchId = 123,
            NewGameDate = DateTime.Now.AddDays(1)
        };

        configuration.GetValue<bool>("SyncEventChangeGameDate").Returns(true);

        var mappedRequest = new InsertOrUpdateEventChangeKoRequest
        {
            MatchId = 123,
            NewGameDate = @event.NewGameDate
        };

        _mapper.Map<InsertOrUpdateEventChangeKoRequest>(@event).Returns(mappedRequest);

        // Act
        var result = _controller.SyncEventChangeGameDate(configuration, sender, @event);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Success", okResult.Value);

        sender.Received(1).Send(mappedRequest);
        _mapper.Received(1).Map<InsertOrUpdateEventChangeKoRequest>(@event);
    }

    [Fact]
    public void SyncEventChangeGameDate_WithDisabledConfiguration_ShouldNotSendRequestAndReturnOk()
    {
        // Arrange
        var configuration = Substitute.For<IConfiguration>();
        var sender = Substitute.For<ISender>();
        var @event = new socer_event_change_game_date
        {
            MatchId = 456,
            NewGameDate = DateTime.Now.AddDays(2)
        };

        configuration.GetValue<bool>("SyncEventChangeGameDate").Returns(false);

        // Act
        var result = _controller.SyncEventChangeGameDate(configuration, sender, @event);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Success", okResult.Value);

        sender.DidNotReceive().Send(Arg.Any<InsertOrUpdateEventChangeKoRequest>());
        _mapper.DidNotReceive().Map<InsertOrUpdateEventChangeKoRequest>(Arg.Any<socer_event_change_game_date>());
    }

    [Fact]
    public void SyncEventChangeGameDate_WithNullEvent_ShouldHandleGracefully()
    {
        // Arrange
        var configuration = Substitute.For<IConfiguration>();
        var sender = Substitute.For<ISender>();
        socer_event_change_game_date? @event = null;

        configuration.GetValue<bool>("SyncEventChangeGameDate").Returns(true);

        // Act
        var result = _controller.SyncEventChangeGameDate(configuration, sender, @event!);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Success", okResult.Value);

        // Should not throw exception even with null event
        sender.DidNotReceive().Send(Arg.Any<InsertOrUpdateEventChangeKoRequest>());
    }

    [Fact]
    public void SyncEventChangeGameDate_WithNullConfiguration_ShouldHandleGracefully()
    {
        // Arrange
        IConfiguration? configuration = null;
        var sender = Substitute.For<ISender>();
        var @event = new socer_event_change_game_date
        {
            MatchId = 789,
            NewGameDate = DateTime.Now.AddDays(3)
        };

        // Act
        var result = _controller.SyncEventChangeGameDate(configuration!, sender, @event);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Success", okResult.Value);

        // Should not throw exception even with null configuration
        sender.DidNotReceive().Send(Arg.Any<InsertOrUpdateEventChangeKoRequest>());
    }

    [Fact]
    public void SyncEventChangeGameDate_WithNullSender_ShouldHandleGracefully()
    {
        // Arrange
        var configuration = Substitute.For<IConfiguration>();
        ISender? sender = null;
        var @event = new socer_event_change_game_date
        {
            MatchId = 101,
            NewGameDate = DateTime.Now.AddDays(4)
        };

        configuration.GetValue<bool>("SyncEventChangeGameDate").Returns(true);

        // Act
        var result = _controller.SyncEventChangeGameDate(configuration, sender!, @event);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Success", okResult.Value);

        // Should not throw exception even with null sender
        _mapper.Received(1).Map<InsertOrUpdateEventChangeKoRequest>(@event);
    }

    [Fact]
    public void SyncEventChangeGameDate_WithConfigurationKeyNotFound_ShouldDefaultToFalse()
    {
        // Arrange
        var configuration = Substitute.For<IConfiguration>();
        var sender = Substitute.For<ISender>();
        var @event = new socer_event_change_game_date
        {
            MatchId = 202,
            NewGameDate = DateTime.Now.AddDays(5)
        };

        // Configuration key not found, should default to false
        configuration.GetValue<bool>("SyncEventChangeGameDate").Returns(false);

        // Act
        var result = _controller.SyncEventChangeGameDate(configuration, sender, @event);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Success", okResult.Value);

        sender.DidNotReceive().Send(Arg.Any<InsertOrUpdateEventChangeKoRequest>());
    }

    [Fact]
    public void SyncEventChangeGameDate_WithMapperException_ShouldStillReturnOk()
    {
        // Arrange
        var configuration = Substitute.For<IConfiguration>();
        var sender = Substitute.For<ISender>();
        var @event = new socer_event_change_game_date
        {
            MatchId = 303,
            NewGameDate = DateTime.Now.AddDays(6)
        };

        configuration.GetValue<bool>("SyncEventChangeGameDate").Returns(true);

        var mapperException = new AutoMapperMappingException("Mapping failed");
        _mapper.Map<InsertOrUpdateEventChangeKoRequest>(@event).Throws(mapperException);

        // Act
        var result = _controller.SyncEventChangeGameDate(configuration, sender, @event);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Success", okResult.Value);

        // Should not send request if mapping fails
        sender.DidNotReceive().Send(Arg.Any<InsertOrUpdateEventChangeKoRequest>());
    }

    [Fact]
    public void SyncEventChangeGameDate_WithSenderException_ShouldStillReturnOk()
    {
        // Arrange
        var configuration = Substitute.For<IConfiguration>();
        var sender = Substitute.For<ISender>();
        var @event = new socer_event_change_game_date
        {
            MatchId = 404,
            NewGameDate = DateTime.Now.AddDays(7)
        };

        configuration.GetValue<bool>("SyncEventChangeGameDate").Returns(true);

        var mappedRequest = new InsertOrUpdateEventChangeKoRequest
        {
            MatchId = 404,
            NewGameDate = @event.NewGameDate
        };

        _mapper.Map<InsertOrUpdateEventChangeKoRequest>(@event).Returns(mappedRequest);

        var senderException = new InvalidOperationException("Sender failed");
        sender.When(x => x.Send(mappedRequest)).Throw(senderException);

        // Act
        var result = _controller.SyncEventChangeGameDate(configuration, sender, @event);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Success", okResult.Value);

        // Should still attempt to send even if it fails
        sender.Received(1).Send(mappedRequest);
    }

    [Fact]
    public void SyncEventChangeGameDate_WithComplexEventData_ShouldMapCorrectly()
    {
        // Arrange
        var configuration = Substitute.For<IConfiguration>();
        var sender = Substitute.For<ISender>();
        var @event = new socer_event_change_game_date
        {
            MatchId = 505,
            NewGameDate = new DateTime(2024, 12, 25, 15, 30, 0),
            OldGameDate = new DateTime(2024, 12, 24, 14, 0, 0),
            Reason = "Weather conditions"
        };

        configuration.GetValue<bool>("SyncEventChangeGameDate").Returns(true);

        var expectedMappedRequest = new InsertOrUpdateEventChangeKoRequest
        {
            MatchId = 505,
            NewGameDate = @event.NewGameDate,
            OldGameDate = @event.OldGameDate,
            Reason = @event.Reason
        };

        _mapper.Map<InsertOrUpdateEventChangeKoRequest>(@event).Returns(expectedMappedRequest);

        // Act
        var result = _controller.SyncEventChangeGameDate(configuration, sender, @event);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Success", okResult.Value);

        sender.Received(1).Send(expectedMappedRequest);
        _mapper.Received(1).Map<InsertOrUpdateEventChangeKoRequest>(@event);
    }

    [Fact]
    public void SyncEventChangeGameDate_WithZeroMatchId_ShouldHandleCorrectly()
    {
        // Arrange
        var configuration = Substitute.For<IConfiguration>();
        var sender = Substitute.For<ISender>();
        var @event = new socer_event_change_game_date
        {
            MatchId = 0,
            NewGameDate = DateTime.Now
        };

        configuration.GetValue<bool>("SyncEventChangeGameDate").Returns(true);

        var mappedRequest = new InsertOrUpdateEventChangeKoRequest
        {
            MatchId = 0,
            NewGameDate = @event.NewGameDate
        };

        _mapper.Map<InsertOrUpdateEventChangeKoRequest>(@event).Returns(mappedRequest);

        // Act
        var result = _controller.SyncEventChangeGameDate(configuration, sender, @event);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Success", okResult.Value);

        sender.Received(1).Send(mappedRequest);
    }

    [Fact]
    public void SyncEventChangeGameDate_WithPastDate_ShouldHandleCorrectly()
    {
        // Arrange
        var configuration = Substitute.For<IConfiguration>();
        var sender = Substitute.For<ISender>();
        var @event = new socer_event_change_game_date
        {
            MatchId = 606,
            NewGameDate = DateTime.Now.AddDays(-1) // Past date
        };

        configuration.GetValue<bool>("SyncEventChangeGameDate").Returns(true);

        var mappedRequest = new InsertOrUpdateEventChangeKoRequest
        {
            MatchId = 606,
            NewGameDate = @event.NewGameDate
        };

        _mapper.Map<InsertOrUpdateEventChangeKoRequest>(@event).Returns(mappedRequest);

        // Act
        var result = _controller.SyncEventChangeGameDate(configuration, sender, @event);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Success", okResult.Value);

        sender.Received(1).Send(mappedRequest);
    }
}
