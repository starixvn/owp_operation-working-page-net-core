using NSubstitute;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Services;
using Xunit;

namespace OWP.TicketCenter.UnitTests.Services;

public class RecipientServiceUnitTests
{
    private readonly Repository repository;

    public RecipientServiceUnitTests()
    {
        repository = Substitute.For<Repository>();
    }

    [Fact]
    public async Task InsertManyRecipient_Always_CallAddNewRecipients()
    {
        var service = new RecipientServiceImp(repository);
        var recipients = MockListRecipients();
        var ticketId = 12;

        await service.InsertManyRecipient(recipients, ticketId);

        await repository.Received().AddManyRecipients(Arg.Compat.Any<AddManyRecipientCriteria>());
        await repository.Received().AddManyRecipients(Arg.Compat.Any<AddManyRecipientCriteria>());
    }

    [Fact]
    public async Task RemoveManyRecipientReader_Always_CallRemoveManyRecipientReader()
    {
        var service = new RecipientServiceImp(repository);
        var recipients = MockListRecipients();
        var ticketId = 12;

        await service.RemoveManyRecipientReader(recipients, ticketId);

        await repository.Received().RemoveManyRecipientReader(
            Arg.Is<RemoveManyRecipientReaderCriteria>(x => CompareHelper.Compare(x, new RemoveManyRecipientReaderCriteria
            {
                GroupIds = "4,5,6",
                IndividualIds = "1",
                TicketId = 12
            })));
    }

    [Fact]
    public async Task InsertManyRecipientReader_Always_CallInsertManyRecipientReader()
    {
        var service = new RecipientServiceImp(repository);
        var recipients = MockListRecipients();
        var ticketId = 10;
        var creatorId = 8;

        await service.InsertManyRecipientReader(recipients, ticketId, creatorId);

        await repository.Received().InsertManyRecipientReader(
            Arg.Is<InsertManyRecipientReaderCriteria>(x => CompareHelper.Compare(x, new InsertManyRecipientReaderCriteria
            {
                GroupIds = "4,5,6",
                IndividualIds = "1",
                TicketId = 10,
                CreatorId = 8
            })));
    }

    [Fact]
    public List<InsertOrRemoveManyRecipientModel> MockListRecipients()
        => new()
        {
            new InsertOrRemoveManyRecipientModel
            {
                RecipientIds =new List<int> {1},
                RecipientType = 1
            },
            new InsertOrRemoveManyRecipientModel
            {
                RecipientIds =new List<int> {4,5,6},
                RecipientType = 2
            }
        };
}