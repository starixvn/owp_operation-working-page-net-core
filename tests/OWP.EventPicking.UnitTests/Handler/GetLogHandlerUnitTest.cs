using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NSubstitute;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Handler;

public class GetLogHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldCallLogService_AndReturnLogs()
    {
        // Arrange
        var logService = Substitute.For<LogService>();
        var handler = new GetLogHandler(logService);

        var request = new GetLogRequest
        {
            InfoSiteMatchId = 12345
        };

        var expectedLogs = new List<LogModel>
        {
            new() { Date = System.DateTime.Now, ModifiedBy = "user1", Action = "Test Action 1" },
            new() { Date = System.DateTime.Now, ModifiedBy = "user2", Action = "Test Action 2" }
        };

        logService.GetLog(request.InfoSiteMatchId).Returns(expectedLogs);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await logService.Received(1).GetLog(12345);
        Assert.Equal(expectedLogs, result);
    }
}