using System.Threading;
using System.Threading.Tasks;
using Fanex.NAP.Client;
using MediatR;
using NSubstitute;
using OWP.CQRS;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Models;
using Xunit;

namespace OWP.EventPicking.UnitTests.Handler;

public class RemoveSpecialProductHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldExecuteNonQuery_WithMappedValues_AndReturnUnit()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(456);

        var repository = Substitute.For<Repository>();
        var handler = new RemoveSpecialProductHandler(session, repository);

        var removeInfo = new RemoveSpecialProduct
        {
            ParentId = "50,60,70",
            MinorBettypeId = "201,202,203"
        };
        var request = new RemoveSpecialProductRequest(removeInfo);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQuery(
            Arg.Is<RemoveSpecialProductCriteria>(c =>
                c.ActorId == 456 &&
                c.ParentId == "50,60,70" &&
                c.MinorBettypeId == "201,202,203"));

        Assert.Equal(Unit.Value, result);
    }
}