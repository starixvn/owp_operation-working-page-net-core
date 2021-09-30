namespace OWP.TicketCenter.UnitTests.Services
{
    using Enum;
    using Models;
    using NSubstitute;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TicketCenter.Services;
    using Xunit;

    public class InsertOrUpdateRecipientServiceUnitTests
    {
        private readonly Repository repository;

        public InsertOrUpdateRecipientServiceUnitTests()
        {
            repository = Substitute.For<Repository>();
        }

        [Fact]
        public async Task Execute_AddNewRecipients_CallAddNewRecipients()
        {
            var service = new InsertOrUpdateRecipientServiceImp(repository);
            var recipients = MockListRecipients();
            var ticketId = 12;
            var action = RecipientAction.Add;

            await service.Execute(recipients, ticketId, action);

            await repository.Received().AddNewRecipients(ticketId, Arg.Compat.Any<string>(), 1);
            await repository.Received().AddNewRecipients(ticketId, Arg.Compat.Any<string>(), 2);
        }

        [Fact]
        public async Task Execute_DeleteRecipients_CallDeleteRecipients()
        {
            var service = new InsertOrUpdateRecipientServiceImp(repository);
            var recipients = MockListRecipients();
            var ticketId = 12;
            var action = RecipientAction.Delete;

            await service.Execute(recipients, ticketId, action);

            await repository.Received().DeleteRecipients(ticketId, Arg.Compat.Any<string>(), 1);
            await repository.Received().DeleteRecipients(ticketId, Arg.Compat.Any<string>(), 2);
        }

        public List<AddNewRecipientModel> MockListRecipients()
            => new List<AddNewRecipientModel>
            {
                new()
                {
                    RecipientIds =new List<int>(){1,2,3},
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