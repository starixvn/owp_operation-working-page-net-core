using System;
using System.Threading;
using System.Threading.Tasks;
using Fanex.NAP.Client;
using MediatR;
using NSubstitute;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Handlers;
using Xunit;

namespace OWP.EventPicking.UnitTests.Handler;

public class UpdateConfirmedHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldConvertTimestampToGmt8_AndCallRepository()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        session.UserName.Returns("testuser");

        var handler = new UpdateConfirmedHandler(repository, session);

        var request = new UpdateConfirmedRequest
        {
            InfoSiteMatchIds = "123,456,789",
            Confirmed = true,
            TimestampConfirm = 1640995200000 // 2022-01-01 00:00:00 UTC
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).UpdateConfirmed(
            Arg.Is<UpdateConfirmedMatchCriteria>(c =>
                c.InfoSiteMatchIds == "123,456,789" &&
                c.ModifiedBy == "testuser" &&
                c.Confirmed == true &&
                c.ConfirmedAt.Date == new DateTime(2022, 1, 1, 8, 0, 0).Date)); // GMT+8

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldHandleFalseConfirmed_AndCallRepository()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        session.UserName.Returns("admin");

        var handler = new UpdateConfirmedHandler(repository, session);

        var request = new UpdateConfirmedRequest
        {
            InfoSiteMatchIds = "999",
            Confirmed = false,
            TimestampConfirm = 1640995200000
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).UpdateConfirmed(
            Arg.Is<UpdateConfirmedMatchCriteria>(c =>
                c.InfoSiteMatchIds == "999" &&
                c.ModifiedBy == "admin" &&
                c.Confirmed == false));

        Assert.Equal(Unit.Value, result);
    }
}