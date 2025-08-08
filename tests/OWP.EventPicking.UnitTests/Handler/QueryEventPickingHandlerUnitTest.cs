using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NSubstitute;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Middleware.Models;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Handler;

public class QueryEventPickingHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldCallServiceWithExcludedUpdateTypes_AndReturnResult()
    {
        // Arrange
        var queryService = Substitute.For<QueryEventPickingService>();
        var handler = new QueryEventPickingHandler(queryService);

        var request = new QueryEventPickingRequest
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

        var expectedResult = new EventPickingResult();
        queryService.Execute(request, Arg.Any<short[]>()).Returns(expectedResult);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await queryService.Received(1).Execute(request, Arg.Is<short[]>(excludedTypes =>
            excludedTypes.Length == 12));

        Assert.Equal(expectedResult, result);
    }
}