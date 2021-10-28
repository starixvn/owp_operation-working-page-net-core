using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Services;
using Xunit;

namespace OWP.TicketCenter.UnitTests.Handlers
{
    public class InsertSingleRecipientHandlerUnitTests
    {
        private readonly InsertRecipientHandler handler;

        private readonly Repository repository;

        private readonly RecipientService recipientService;

        public InsertSingleRecipientHandlerUnitTests()
        {
            repository = Substitute.For<Repository>();
            recipientService = Substitute.For<RecipientService>();
        }

        [Fact]
        public async void InsertSingleRecipient_RecipientInfo_ReturnIdFromDB()
        {
            // Arrange
            var request = new InsertSingleRecipientRequest();

            var recipients = new List<InsertOrRemoveManyRecipientModel>();

            recipientService
                .InsertManyRecipientReader(recipients, 1)
                .Returns(x => true);

            repository
                .AddSingleRecipient(request)
                .ReturnsForAnyArgs(x => 1);

            var actual = await handler.Handle(request, CancellationToken.None);

            //Assert
            var expected = 1;
            Assert.Equal(actual, expected);
        }
    }
}