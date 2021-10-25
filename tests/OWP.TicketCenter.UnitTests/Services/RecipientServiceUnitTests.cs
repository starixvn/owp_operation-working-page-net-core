namespace OWP.TicketCenter.UnitTests.Services
{
    using Models;
    using NSubstitute;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TicketCenter.Services;
    using Xunit;

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

            await repository.Received().AddManyRecipients(ticketId, Arg.Compat.Any<string>(), 1);
            await repository.Received().AddManyRecipients(ticketId, Arg.Compat.Any<string>(), 2);
        }

        [Fact]
        public async Task RemoveManyRecipientReader_Always_CallRemoveManyRecipientReader()
        {
            repository.GetRecipientIdInManyGroup("4,5,6").Returns(new List<int>() { 2, 3, 4, 5, 6 });
            var service = new RecipientServiceImp(repository);
            var recipients = MockListRecipients();
            var ticketId = 12;

            await service.RemoveManyRecipientReader(recipients, ticketId);

            await repository.Received().RemoveManyRecipientReader(Arg.Is<string>(x => x.Equals("2,3,4,5,6,1")), ticketId);
        }

        [Fact]
        public async Task InsertManyRecipientReader_Always_CallInsertManyRecipientReader()
        {
            repository.GetRecipientIdInManyGroup("4,5,6").Returns(new List<int>() { 3, 4, 5, 6 });
            var service = new RecipientServiceImp(repository);
            var recipients = MockListRecipients();
            var ticketId = 10;

            await service.InsertManyRecipientReader(recipients, ticketId);

            await repository.Received().InsertManyRecipientReader(Arg.Is<string>(x => x.Equals("3,4,5,6,1")), ticketId, false);
        }

        [Fact]
        public List<InsertOrRemoveManyRecipientModel> MockListRecipients()
            => new List<InsertOrRemoveManyRecipientModel>
            {
                new()
                {
                    RecipientIds =new List<int>(){1},
                    RecipientType = 1
                },
                new()
                {
                    RecipientIds =new List<int>(){4,5,6},
                    RecipientType = 2
                },
            };
    }
}