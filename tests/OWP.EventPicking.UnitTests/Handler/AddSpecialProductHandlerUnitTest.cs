using System.Threading;
using System.Threading.Tasks;
using Fanex.NAP.Client;
using MediatR;
using NSubstitute;
using OWP.CQRS;
using OWP.EventPicking;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Models;
using Xunit;

namespace OWP.EventPicking.UnitTests.Handler;

public class AddSpecialProductHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldExecuteNonQuery_WithMappedValues_AndReturnUnit()
    {
        // Arrange
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(123);

        var repository = Substitute.For<Repository>();
        var handler = new AddSpecialProductHandler(session, repository);

        var addInfo = new AddSpecialProduct
        {
            ParentId = "10,20,30",
            MinorBettypeId = "101,102"
        };
        var request = new AddSpecialProductRequest(addInfo);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQuery(
            Arg.Is<AddSpecialProductCriteria>(c =>
                c.ActorId == 123 &&
                c.ParentId == "10,20,30" &&
                c.MinorBettypeId == "101,102"));

        Assert.Equal(Unit.Value, result);
    }
}