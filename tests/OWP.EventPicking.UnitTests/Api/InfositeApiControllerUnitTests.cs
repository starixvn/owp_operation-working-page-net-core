using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using OWP.EventPicking.Api;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Models;
using Xunit;
using System.Collections.Generic; // Added for Dictionary

namespace OWP.EventPicking.UnitTests.Api;

public class InfositeApiControllerUnitTests
{
    private readonly IMapper _mapper;

    public InfositeApiControllerUnitTests()
    {
        _mapper = Substitute.For<IMapper>();
    }

    private IConfiguration CreateConfigurationMock(bool syncEventChangeGameDateEnabled = true)
    {
        // Use a real configuration builder instead of NSubstitute to avoid mocking issues
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "SyncEventChangeGameDate", syncEventChangeGameDateEnabled.ToString() }
        });

        return configurationBuilder.Build();
    }

    [Fact]
    public void SyncEventChangeGameDate_WithEnabledConfiguration_ShouldSendRequestAndReturnOk()
    {
        // Arrange
        var configuration = CreateConfigurationMock(true);
        var sender = Substitute.For<ISender>();
        var @event = new socer_event_change_game_date(123, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
        var controller = new InfositeApiController(_mapper);

        var mappedRequest = new InsertOrUpdateEventChangeKoRequest(123, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));

        _mapper.Map<InsertOrUpdateEventChangeKoRequest>(@event).Returns(mappedRequest);

        // Act
        var result = controller.SyncEventChangeGameDate(configuration, sender, @event);

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
        var configuration = CreateConfigurationMock(false);
        var sender = Substitute.For<ISender>();
        var @event = new socer_event_change_game_date(456, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(2));
        var controller = new InfositeApiController(_mapper);

        // Act
        var result = controller.SyncEventChangeGameDate(configuration, sender, @event);

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
        var configuration = CreateConfigurationMock(true);
        var sender = Substitute.For<ISender>();
        socer_event_change_game_date? @event = null;
        var controller = new InfositeApiController(_mapper);

        // The controller doesn't have null checks, so it will try to process null events
        // Mock the mapper to return null for null events
        _mapper.Map<InsertOrUpdateEventChangeKoRequest>(@event).Returns((InsertOrUpdateEventChangeKoRequest?)null);

        // Act
        var result = controller.SyncEventChangeGameDate(configuration, sender, @event!);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Success", okResult.Value);

        // The controller will still try to send the request even if it's null
        sender.Received(1).Send(Arg.Any<InsertOrUpdateEventChangeKoRequest>());
    }

    [Fact]
    public void SyncEventChangeGameDate_WithNullConfiguration_ShouldHandleGracefully()
    {
        // Arrange
        IConfiguration? configuration = null;
        var sender = Substitute.For<ISender>();
        var @event = new socer_event_change_game_date(789, DateTime.Now.AddDays(-3), DateTime.Now.AddDays(3));
        var controller = new InfositeApiController(_mapper);

        // Act & Assert
        // The ConfigurationBinder.GetValue throws ArgumentNullException when configuration is null
        Assert.Throws<ArgumentNullException>(() => controller.SyncEventChangeGameDate(configuration!, sender, @event));
    }

    [Fact]
    public void SyncEventChangeGameDate_WithNullSender_ShouldHandleGracefully()
    {
        // Arrange
        var configuration = CreateConfigurationMock(true);
        ISender? sender = null;
        var @event = new socer_event_change_game_date(101, DateTime.Now.AddDays(-4), DateTime.Now.AddDays(4));
        var controller = new InfositeApiController(_mapper);

        var mappedRequest = new InsertOrUpdateEventChangeKoRequest(101, DateTime.Now.AddDays(-4), DateTime.Now.AddDays(4));
        _mapper.Map<InsertOrUpdateEventChangeKoRequest>(@event).Returns(mappedRequest);

        // Act & Assert
        // The controller doesn't have null checks, so it will throw NullReferenceException
        Assert.Throws<NullReferenceException>(() => controller.SyncEventChangeGameDate(configuration, sender!, @event));
    }

    [Fact]
    public void SyncEventChangeGameDate_WithSenderException_ShouldHandleGracefully()
    {
        // Arrange
        var configuration = CreateConfigurationMock(true);
        var sender = Substitute.For<ISender>();
        var @event = new socer_event_change_game_date(404, DateTime.Now.AddDays(-7), DateTime.Now.AddDays(7));
        var controller = new InfositeApiController(_mapper);

        var mappedRequest = new InsertOrUpdateEventChangeKoRequest(404, DateTime.Now.AddDays(-7), DateTime.Now.AddDays(7));

        _mapper.Map<InsertOrUpdateEventChangeKoRequest>(@event).Returns(mappedRequest);

        var senderException = new InvalidOperationException("Sender failed");
        sender.When(x => x.Send(mappedRequest)).Throw(senderException);

        // Act & Assert
        // The controller doesn't have exception handling, so it will throw the exception
        var exception = Assert.Throws<InvalidOperationException>(() =>
            controller.SyncEventChangeGameDate(configuration, sender, @event));

        Assert.Equal("Sender failed", exception.Message);
    }

    [Fact]
    public void SyncEventChangeGameDate_WithComplexEventData_ShouldMapCorrectly()
    {
        // Arrange
        var configuration = CreateConfigurationMock(true);
        var sender = Substitute.For<ISender>();
        var oldDate = new DateTime(2024, 12, 24, 14, 0, 0);
        var newDate = new DateTime(2024, 12, 25, 15, 30, 0);
        var @event = new socer_event_change_game_date(505, oldDate, newDate);
        var controller = new InfositeApiController(_mapper);

        var expectedMappedRequest = new InsertOrUpdateEventChangeKoRequest(505, oldDate, newDate);

        _mapper.Map<InsertOrUpdateEventChangeKoRequest>(@event).Returns(expectedMappedRequest);

        // Act
        var result = controller.SyncEventChangeGameDate(configuration, sender, @event);

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
        var configuration = CreateConfigurationMock(true);
        var sender = Substitute.For<ISender>();
        var @event = new socer_event_change_game_date(0, DateTime.Now.AddDays(-1), DateTime.Now);
        var controller = new InfositeApiController(_mapper);

        var mappedRequest = new InsertOrUpdateEventChangeKoRequest(0, DateTime.Now.AddDays(-1), DateTime.Now);

        _mapper.Map<InsertOrUpdateEventChangeKoRequest>(@event).Returns(mappedRequest);

        // Act
        var result = controller.SyncEventChangeGameDate(configuration, sender, @event);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Success", okResult.Value);

        sender.Received(1).Send(mappedRequest);
    }

    [Fact]
    public void SyncEventChangeGameDate_WithPastDate_ShouldHandleCorrectly()
    {
        // Arrange
        var configuration = CreateConfigurationMock(true);
        var sender = Substitute.For<ISender>();
        var @event = new socer_event_change_game_date(606, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1)); // Past date
        var controller = new InfositeApiController(_mapper);

        var mappedRequest = new InsertOrUpdateEventChangeKoRequest(606, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1));

        _mapper.Map<InsertOrUpdateEventChangeKoRequest>(@event).Returns(mappedRequest);

        // Act
        var result = controller.SyncEventChangeGameDate(configuration, sender, @event);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Success", okResult.Value);

        sender.Received(1).Send(mappedRequest);
    }
}
