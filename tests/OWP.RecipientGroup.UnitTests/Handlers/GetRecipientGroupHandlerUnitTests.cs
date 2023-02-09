namespace OWP.RecipientGroup.UnitTests.Handlers;

using Models;
using NSubstitute;
using OWP.RecipientGroup.Handlers;
using Services;

using Xunit;
using Xunit.Asserts.Compare;

public class GetRecipientGroupHandlerUnitTests
{
    private readonly CommandRepository commandRepository;
    private readonly GetRecipientGroupHandler handler;

    public GetRecipientGroupHandlerUnitTests()
    {
        commandRepository = Substitute.For<CommandRepository>();
        handler = new GetRecipientGroupHandler(commandRepository);
    }

    [Fact]
    public async Task GetRecipientGroupHandler()
    {
        var mockData = new List<RecipientGroupModel>
        {
            new(){ RecipientGroupId = 1, GroupId = 1, GroupName = "LST", UserId = 111, Username = "sa" },
            new(){ RecipientGroupId = 2, GroupId = 1, GroupName = "LST", UserId = 222, Username = "carlos" },
            new(){ RecipientGroupId = 3, GroupId = 1, GroupName = "LST", UserId = 333, Username = "tyler" },
            new(){ RecipientGroupId = 4, GroupId = 3, GroupName = "Candy", UserId = 444, Username = "mira" },
            new(){ RecipientGroupId = 5, GroupId = 3, GroupName = "Candy", UserId = 555, Username = "luca" },
            new(){ RecipientGroupId = 6, GroupId = 4, GroupName = "Test", UserId = 666, Username = "liam" },
        };
        var expected = new List<TicketGroup>
        {
            new()
            {
                GroupId = 3,
                GroupName = "Candy",
                Recipients = new List<Recipient>
                {
                    new() { RecipientGroupId = 5, RecipientId = 555, RecipientType = 1, RecipientName = "luca" },
                    new() { RecipientGroupId = 4, RecipientId = 444, RecipientType = 1, RecipientName = "mira" }
                }
            },
            new()
            {
                GroupId = 1,
                GroupName = "LST",
                Recipients = new List<Recipient>
                {
                    new() { RecipientGroupId = 2, RecipientId = 222, RecipientType = 1, RecipientName = "carlos" },
                    new() { RecipientGroupId = 1, RecipientId = 111, RecipientType = 1, RecipientName = "sa" },
                    new() { RecipientGroupId = 3, RecipientId = 333, RecipientType = 1, RecipientName = "tyler" }
                }
            },
            new()
            {
                GroupId = 4,
                GroupName = "Test",
                Recipients = new List<Recipient>
                {
                    new() { RecipientGroupId = 6, RecipientId = 666, RecipientType = 1, RecipientName = "liam" }
                }
            }
        };

        commandRepository.GetRecipientGroup().Returns(mockData);

        var actual = await handler.Handle(new QueryRecipientGroupRequest(), new CancellationToken(false));

        DeepAssert.Equal(expected, actual.ToList());
    }
}