using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fanex.Data.Repository;
using MediatR;
using NSubstitute;
using Olympus.OWP.Log.Criteria;
using Olympus.OWP.Log.Services;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Models;
using Xunit;

namespace OWP.EventPicking.UnitTests.Handler;

public class InsertOrUpdateEventChangeKoHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldUpdateEventDateTime_AndSaveLogs_AndReturnUnit()
    {
        // Arrange
        var repository = Substitute.For<IDynamicRepository>();
        var logger = Substitute.For<LogWriterService>();

        var handler = new InsertOrUpdateEventChangeKoHandler(repository, logger);

        var oldDate = new DateTime(2024, 1, 15, 14, 0, 0); // 2 PM
        var newDate = new DateTime(2024, 1, 16, 16, 0, 0); // 4 PM next day

        var request = new InsertOrUpdateEventChangeKoRequest(12345, oldDate, newDate);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteAsync(
            Arg.Is<UpdateEventDateTimeCriteria>(c =>
                c.InfoSiteId == 12345 &&
                c.NewEventDate == newDate.AddHours(8))); // GMT+8

        await logger.Received(2).Execute(Arg.Any<InfoSiteLogCriteria>());

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldHandleException_AndWriteErrorLog_AndRethrow()
    {
        // Arrange
        var repository = Substitute.For<IDynamicRepository>();
        var logger = Substitute.For<LogWriterService>();

        repository.ExecuteAsync(Arg.Any<UpdateEventDateTimeCriteria>())
            .Returns(Task.FromException<int>(new Exception("Database error")));

        var handler = new InsertOrUpdateEventChangeKoHandler(repository, logger);

        var oldDate = new DateTime(2024, 1, 15, 14, 0, 0);
        var newDate = new DateTime(2024, 1, 16, 16, 0, 0);

        var request = new InsertOrUpdateEventChangeKoRequest(12345, oldDate, newDate);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));

        await logger.Received(2).Execute(Arg.Any<InfoSiteLogCriteria>());
    }
}