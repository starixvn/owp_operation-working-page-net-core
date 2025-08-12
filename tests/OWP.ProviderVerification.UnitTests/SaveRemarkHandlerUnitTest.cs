using System.Threading;
using System.Threading.Tasks;
using Fanex.NAP.Client;
using MediatR;
using NSubstitute;
using OWP.ProviderVerification.ProviderVerification.SaveRemark;
using Xunit;

namespace OWP.ProviderVerification.UnitTests;

public class SaveRemarkHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldCallRepositorySaveRemark_AndReturnUnit()
    {
        // Arrange
        var repository = Substitute.For<ProviderVerificationRepository>();
        var session = Substitute.For<INapSession>();
        var handler = new SaveRemarkHandler(repository, session);

        var request = new SaveRemarkRequest
        {
            OwpMatchId = 123,
            Remark = "Test remark"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).SaveRemark(123, "Test remark");
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithDifferentValues_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange
        var repository = Substitute.For<ProviderVerificationRepository>();
        var session = Substitute.For<INapSession>();
        var handler = new SaveRemarkHandler(repository, session);

        var request = new SaveRemarkRequest
        {
            OwpMatchId = 456,
            Remark = "Another test remark"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).SaveRemark(456, "Another test remark");
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithEmptyRemark_ShouldCallRepositoryWithEmptyString()
    {
        // Arrange
        var repository = Substitute.For<ProviderVerificationRepository>();
        var session = Substitute.For<INapSession>();
        var handler = new SaveRemarkHandler(repository, session);

        var request = new SaveRemarkRequest
        {
            OwpMatchId = 789,
            Remark = ""
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).SaveRemark(789, "");
        Assert.Equal(Unit.Value, result);
    }
}