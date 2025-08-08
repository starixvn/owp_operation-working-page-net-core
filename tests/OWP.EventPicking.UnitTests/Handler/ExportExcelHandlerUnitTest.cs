using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Middleware.Models;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Handler;

public class ExportExcelHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldFilterMatches_AndCallExportService_AndReturnFileResult()
    {
        // Arrange
        var queryService = Substitute.For<QueryEventPickingService>();
        var exportService = Substitute.For<ExportService>();

        var handler = new ExportExcelHandler(queryService, exportService);

        var request = new ExportExcelRequest
        {
            Date = new DateTime(2024, 1, 15),
            SelectGame = new List<int> { 1, 2, 3 },
            FilterMatch = new List<string> { "filter1" },
            HasProvider = new List<string> { "yes" },
            HasOtherProvider = new List<string> { "no" },
            FilterMatchCategory = new List<int> { 1 },
            UpdateType = new List<string> { "type1" },
            Period = new Period
            {
                OneMonth = new TimeRange { FromDate = DateTime.Now.AddMonths(-1), ToDate = DateTime.Now },
                ThreeMonths = new TimeRange { FromDate = DateTime.Now.AddMonths(-3), ToDate = DateTime.Now },
                SixMonths = new TimeRange { FromDate = DateTime.Now.AddMonths(-6), ToDate = DateTime.Now },
                TwelveMonths = new TimeRange { FromDate = DateTime.Now.AddMonths(-12), ToDate = DateTime.Now }
            }
        };

        var matches = new EventPickingResult
        {
            FilteredMatches = new List<EventPickingModel>
            {
                new() { InfoSiteMatchId = 1, MinorBettypeId = null }, // Main match
                new() { InfoSiteMatchId = 2, MinorBettypeId = 123 }, // Child match
                new() { InfoSiteMatchId = 3, MinorBettypeId = null } // Main match
            }
        };

        // Create a real FileStreamResult with a MemoryStream
        var memoryStream = new MemoryStream();
        var expectedFileResult = new FileStreamResult(memoryStream, "application/octet-stream");
        
        queryService.Execute(request, Arg.Any<List<short>>()).Returns(matches);
        exportService.BuildFileStream(Arg.Any<IList<EventPickingModel>>(), false).Returns(expectedFileResult);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await queryService.Received(1).Execute(request, Arg.Is<List<short>>(list => list.Count == 0));
        await exportService.Received(1).BuildFileStream(
            Arg.Is<IList<EventPickingModel>>(filteredMatches =>
                filteredMatches.Count == 2), // Only main matches
            false);

        Assert.Equal(expectedFileResult, result);
    }
}