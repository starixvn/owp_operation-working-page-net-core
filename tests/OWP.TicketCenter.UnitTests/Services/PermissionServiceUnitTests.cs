using NSubstitute;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Services;
using Xunit;

namespace OWP.TicketCenter.UnitTests.Services;

public class PermissionServiceUnitTests
{
    private readonly Repository repository;

    public PermissionServiceUnitTests()
    {
        repository = Substitute.For<Repository>();
    }

    [Theory]
    [InlineData(17, 1, true)]
    [InlineData(17, 3, true)]
    [InlineData(17, 6, false)]
    public async Task HasPermission_Always_CheckPermission(int ticketId, int creatorId, bool expected)
    {
        repository.GetUserIdsByTicketId(ticketId).Returns(["1", "2", "3", "4", "5"]);
        var service = new PermissionServiceImp(repository);

        var actual = await service.HasPermission(ticketId, creatorId);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task HasPermissionRemoveLastRecipient_Always_CheckPermission()
    {
        repository.GetRecipientByTicketId(Arg.Any<int>())
            .Returns(new List<RecipientResponseModel> { new() { GroupName = "Liam", RecipientId = 10 } });
        var service = new PermissionServiceImp(repository);

        var actual = await service.HasPermissionRemoveLastRecipient(5);

        Assert.False(actual);
    }


    [Fact]
    public async Task HasExistedTicket_Always_CheckPermission()
    {
        repository.QueryExistedTicketId(Arg.Any<int>()).Returns(true);
        var service = new PermissionServiceImp(repository);

        var actual = await service.HasExistedTicket(5);

        Assert.True(actual);
    }
}