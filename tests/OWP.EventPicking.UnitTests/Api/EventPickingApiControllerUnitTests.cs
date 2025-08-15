using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using OWP.EventPicking;
using OWP.EventPicking.Api;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Models;
using Xunit;

namespace OWP.EventPicking.UnitTests.Api;

public class EventPickingApiControllerUnitTests
{
    private readonly IMediator _mediator;
    private readonly OWP.EventPicking.Repository _repository;
    private readonly EventPickingApiController _controller;

    public EventPickingApiControllerUnitTests()
    {
        _mediator = Substitute.For<IMediator>();
        _repository = Substitute.For<OWP.EventPicking.Repository>();
        _controller = new EventPickingApiController(_mediator, _repository);
    }

    [Fact]
    public async Task QueryEventPicking_ShouldReturnJsonResult()
    {
        // Arrange
        var request = new QueryEventPickingRequest
        {
            Date = DateTime.Now,
            SelectGame = new List<int> { 1, 2, 3 }
        };
        var expectedResult = new EventPickingResult();

        _mediator.Send(request, Arg.Any<CancellationToken>())
            .Returns(expectedResult);

        // Act
        var result = await _controller.QueryEventPicking(request);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.Equal(expectedResult, jsonResult.Value);

        await _mediator.Received(1).Send(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task QueryEventPickingEditMode_ShouldReturnJsonResult()
    {
        // Arrange
        var request = new EditEventPickingRequest
        {
            Date = DateTime.Now,
            SelectGame = new List<int> { 1, 2, 3 }
        };
        var expectedResult = new EventPickingResult();

        _mediator.Send(request, Arg.Any<CancellationToken>())
            .Returns(expectedResult);

        // Act
        var result = await _controller.QueryEventPickingEditMode(request);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.Equal(expectedResult, jsonResult.Value);

        await _mediator.Received(1).Send(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateMaxbetSetting_ShouldSendRequestToMediator()
    {
        // Arrange
        var request = new InsertOrDeleteNoOfferMatchRequest
        {
            NoOfferMatch = new List<NoOfferMatchInfo> { new NoOfferMatchInfo { InfoSiteMatchId = 123, IsNoOffer = true } }
        };

        // Act
        await _controller.UpdateMaxbetSetting(request);

        // Assert
        await _mediator.Received(1).Send(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExportMiscMatch_ShouldReturnFileResult()
    {
        // Arrange
        var request = new ExportMiscMatchRequest
        {
            Date = DateTime.Now
        };
        var expectedFileResult = new FileContentResult(new byte[] { 1, 2, 3 }, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        _mediator.Send(request, Arg.Any<CancellationToken>())
            .Returns(expectedFileResult);

        // Act
        var result = await _controller.ExportMiscMatch(request);

        // Assert
        Assert.Equal(expectedFileResult, result);
        await _mediator.Received(1).Send(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetLogHistory_ShouldReturnLogModels()
    {
        // Arrange
        var request = new GetLogRequest
        {
            InfoSiteMatchId = 123
        };
        var expectedLogs = new List<LogModel>
        {
            new LogModel { Date = DateTime.Now, Action = "Log 1" },
            new LogModel { Date = DateTime.Now, Action = "Log 2" }
        };

        _mediator.Send(request, Arg.Any<CancellationToken>())
            .Returns(expectedLogs);

        // Act
        var result = await _controller.GetLogHistory(request);

        // Assert
        Assert.Equal(expectedLogs, result);
        await _mediator.Received(1).Send(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateConfirmed_ShouldSendRequestToMediator()
    {
        // Arrange
        var request = new UpdateConfirmedRequest
        {
            InfoSiteMatchIds = "123,456",
            Confirmed = true,
            TimestampConfirm = DateTimeOffset.Now.ToUnixTimeMilliseconds()
        };

        // Act
        await _controller.UpdateConfirmed(request);

        // Assert
        await _mediator.Received(1).Send(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task QueryUpdateType_ShouldReturnJsonResultFromRepository()
    {
        // Arrange
        var expectedOptions = new List<Option>
        {
            new Option { Value = "Type1" },
            new Option { Value = "Type2" }
        };

        _repository.GetUpdateType().Returns(expectedOptions);

        // Act
        var result = await _controller.QueryUpdateType();

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.Equal(expectedOptions, jsonResult.Value);

        await _repository.Received(1).GetUpdateType();
    }

    [Fact]
    public async Task ExportExcel_ShouldReturnFileResult()
    {
        // Arrange
        var request = new ExportExcelRequest
        {
            Date = DateTime.Now,
            SelectGame = new List<int> { 1, 2, 3 }
        };
        var expectedFileResult = new FileContentResult(new byte[] { 1, 2, 3 }, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        _mediator.Send(request, Arg.Any<CancellationToken>())
            .Returns(expectedFileResult);

        // Act
        var result = await _controller.ExportExcel(request);

        // Assert
        Assert.Equal(expectedFileResult, result);
        await _mediator.Received(1).Send(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateMainEventMaxbetSpread_ShouldSendRequestToMediator()
    {
        // Arrange
        var request = new UpdateEventsRequest
        {
            Events = new List<UpdateEvents>
            {
                new UpdateEvents { InfoSiteMatchId = 123 }
            }
        };

        // Act
        await _controller.UpdateMainEventMaxbetSpread(request);

        // Assert
        await _mediator.Received(1).Send(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateSpecialProductMaxbetSpread_ShouldSendRequestToMediator()
    {
        // Arrange
        var request = new SpecialProductUpdateMaxbetSpreadRequest
        {
            Events = new List<SpecialProductMaxbetSpread>
            {
                new SpecialProductMaxbetSpread { ParentId = 789, MinorBettypeId = 101 }
            }
        };

        // Act
        await _controller.UpdateSpecialProductMaxbetSpread(request);

        // Assert
        await _mediator.Received(1).Send(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RemoveSpecialProduct_ShouldSendRequestToMediator()
    {
        // Arrange
        var removeInfo = new RemoveSpecialProduct { ParentId = "123", MinorBettypeId = "456" };
        var request = new RemoveSpecialProductRequest(removeInfo);

        // Act
        await _controller.RemoveSpecialProduct(request);

        // Assert
        await _mediator.Received(1).Send(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AddSpecialProduct_ShouldSendRequestToMediator()
    {
        // Arrange
        var addInfo = new AddSpecialProduct { ParentId = "123", MinorBettypeId = "456" };
        var request = new AddSpecialProductRequest(addInfo);

        // Act
        await _controller.AddSpecialProduct(request);

        // Assert
        await _mediator.Received(1).Send(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task QueryEventPicking_WithNullRequest_ShouldHandleGracefully()
    {
        // Arrange
        QueryEventPickingRequest? request = null;
        var expectedResult = new EventPickingResult();

        _mediator.Send(Arg.Any<QueryEventPickingRequest>(), Arg.Any<CancellationToken>())
            .Returns(expectedResult);

        // Act
        var result = await _controller.QueryEventPicking(request!);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.Equal(expectedResult, jsonResult.Value);
    }

    [Fact]
    public async Task QueryEventPicking_WithException_ShouldPropagateException()
    {
        // Arrange
        var request = new QueryEventPickingRequest
        {
            Date = DateTime.Now
        };
        var expectedException = new InvalidOperationException("Test exception");

        _mediator.Send(request, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<EventPickingResult>(expectedException));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _controller.QueryEventPicking(request));

        Assert.Equal("Test exception", exception.Message);
    }

    [Fact]
    public async Task QueryUpdateType_WithRepositoryException_ShouldPropagateException()
    {
        // Arrange
        var expectedException = new InvalidOperationException("Repository error");

        _repository.GetUpdateType().Returns(Task.FromException<List<Option>>(expectedException));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _controller.QueryUpdateType());

        Assert.Equal("Repository error", exception.Message);
    }
}
