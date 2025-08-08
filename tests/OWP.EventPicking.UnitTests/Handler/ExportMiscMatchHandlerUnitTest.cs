using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using OWP.Constants;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using OWP.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Handler;

public class ExportMiscMatchHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldBuildMatches_AndExport_AndUpdateCounter_AndReturnFileResult()
    {
        // Arrange
        var buildService = Substitute.For<BuildMainAndChildMatches>();
        var accessCounter = Substitute.For<AccessCounter>();
        var exportService = Substitute.For<ExportService>();

        var handler = new ExportMiscMatchHandler(buildService, accessCounter, exportService);

        var request = new ExportMiscMatchRequest
        {
            Date = new DateTime(2024, 1, 15)
        };

        var matches = new List<EventPickingModel>
        {
            new() { InfoSiteMatchId = 1 },
            new() { InfoSiteMatchId = 2 }
        };

        // Create a real FileStreamResult with a MemoryStream
        var memoryStream = new MemoryStream();
        var expectedFileResult = new FileStreamResult(memoryStream, "application/octet-stream");
        
        buildService.GetMainAndChildMatchEvent(request.Date).Returns(matches);
        exportService.BuildFileStream(Arg.Any<IList<EventPickingModel>>(), true).Returns(expectedFileResult);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await buildService.Received(1).GetMainAndChildMatchEvent(request.Date);
        await exportService.Received(1).BuildFileStream(Arg.Any<IList<EventPickingModel>>(), true);
        await accessCounter.Received(1).UpdateCounter(AccessCounterConst.ExportMiscMatch);

        Assert.Equal(expectedFileResult, result);
        Assert.Equal($"Event Picking-{request.Date:yyyy-MM-dd}.xlsx", result.FileDownloadName);
    }
}