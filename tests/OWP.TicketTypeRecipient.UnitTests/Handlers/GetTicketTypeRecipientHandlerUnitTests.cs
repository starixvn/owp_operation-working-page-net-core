namespace OWP.TicketTypeRecipient.UnitTests.Handlers
{
    using System.Collections.Generic;
    using Xunit;
    using Fanex.Data.Repository;
    using NSubstitute;
    using OWP.TicketTypeRecipient.Handlers;
    using OWP.TicketTypeRecipient.Enums;
    using OWP.TicketTypeRecipient.Models;
    using System.Threading.Tasks;
    using Xunit.Asserts.Compare;

    public class GetTicketTypeRecipientHandlerUnitTests
    {
        [Fact]
        public async Task Handle_ReturnRecipientDataAsync()
        {
            var handler = MockHander();
            var expected = new List<TicketTypeRecipient>
            {
                new TicketTypeRecipient
                {
                    TicketType = new TicketType
                    {
                        TypeId = 1,
                        TypeName = "Event"
                    },
                    Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            TicketTypeRecipientId = 1,
                            RecipientId = 1070340,
                            RecipientName = "starix1",
                            RecipientType = (int)RecipientEnum.USER
                        },
                        new Recipient
                        {
                            TicketTypeRecipientId = 2,
                            RecipientId = 1,
                            RecipientName = "LA",
                            RecipientType = (int)RecipientEnum.GROUP
                        },
                        new Recipient
                        {
                            TicketTypeRecipientId = 3,
                            RecipientId = 15469717,
                            RecipientName = "starix2",
                            RecipientType = (int)RecipientEnum.USER
                        },
                        new Recipient
                        {
                            TicketTypeRecipientId = 4,
                            RecipientId = 2,
                            RecipientName = "CSD",
                            RecipientType = (int)RecipientEnum.GROUP
                        }
                    }
                },
                new TicketTypeRecipient
                {
                    TicketType = new TicketType
                    {
                        TypeId = 2,
                        TypeName = "Event2"
                    },
                    Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            TicketTypeRecipientId = 5,
                            RecipientId = 2,
                            RecipientName = "CSD",
                            RecipientType = (int)RecipientEnum.GROUP
                        }
                    }
                }
            };

            var actual = await handler.Handle(new GetTicketTypeRecipientRequest(), new System.Threading.CancellationToken(false));

            DeepAssert.Equal(expected, actual);
        }

        private GetTicketTypeRecipientHandler MockHander()
        {
            var mockData = new List<GetTicketTypeRecipientHandler.RecipientData>
            {
                new GetTicketTypeRecipientHandler.RecipientData
                {
                    TypeId = 1,
                    TypeName = "Event",
                    TicketTypeRecipientId = 1,
                    RecipientId = 1070340,
                    RecipientName = "starix1",
                    RecipientType = (int)RecipientEnum.USER,
                },
                new GetTicketTypeRecipientHandler.RecipientData
                {
                    TypeId = 1,
                    TypeName = "Event",
                    TicketTypeRecipientId = 2,
                    RecipientId = 1,
                    RecipientName = "LA",
                    RecipientType = (int)RecipientEnum.GROUP,
                },
                new GetTicketTypeRecipientHandler.RecipientData
                {
                    TypeId = 1,
                    TypeName = "Event",
                    TicketTypeRecipientId = 3,
                    RecipientId = 15469717,
                    RecipientName = "starix2",
                    RecipientType = (int)RecipientEnum.USER,
                },
                new GetTicketTypeRecipientHandler.RecipientData
                {
                    TypeId = 1,
                    TypeName = "Event",
                    TicketTypeRecipientId = 4,
                    RecipientId = 2,
                    RecipientName = "CSD",
                    RecipientType = (int)RecipientEnum.GROUP,
                },
                new GetTicketTypeRecipientHandler.RecipientData
                {
                    TypeId = 2,
                    TypeName = "Event2",
                    TicketTypeRecipientId = 5,
                    RecipientId = 2,
                    RecipientName = "CSD",
                    RecipientType = (int)RecipientEnum.GROUP,
                }
            };
            var dynamicRepo = Substitute.For<IDynamicRepository>();
            dynamicRepo.FetchAsync<GetTicketTypeRecipientHandler.RecipientData>(Arg.Any<GetTicketTypeRecipientHandler.GetRecipientDataCriteria>())
                .Returns(mockData);

            return new GetTicketTypeRecipientHandler(dynamicRepo);
        }
    }
}
