using Fanex.Data.Repository;
using NSubstitute;
using OWP.Enums;
using OWP.TicketSetting.TicketTypeRecipient.Handlers;
using OWP.TicketSetting.TicketTypeRecipient.Models;
using Xunit;

namespace OWP.TicketSetting.UnitTests.TicketTypeRecipient.Handlers;

using Xunit.Asserts.Compare;

public class GetTicketTypeRecipientHandlerUnitTests
{
    [Fact]
    public async Task Handle_ReturnRecipientDataAsync()
    {
        var handler = MockHandler();
        var expected = new List<TicketSetting.TicketTypeRecipient.Models.TicketTypeRecipient>
        {
            new()
            {
                TicketType = new TicketType
                {
                    TypeId = 1,
                    TypeName = "type1"
                },
                Recipients =
                [
                    new Recipient
                    {
                        TicketTypeRecipientId = 4,
                        RecipientId = 2,
                        RecipientName = "CSD",
                        RecipientType = (int)TicketRecipient.GROUP
                    },

                    new Recipient
                    {
                        TicketTypeRecipientId = 2,
                        RecipientId = 1,
                        RecipientName = "LA",
                        RecipientType = (int)TicketRecipient.GROUP
                    },

                    new Recipient
                    {
                        TicketTypeRecipientId = 1,
                        RecipientId = 1070340,
                        RecipientName = "starix1",
                        RecipientType = (int)TicketRecipient.USER
                    },

                    new Recipient
                    {
                        TicketTypeRecipientId = 3,
                        RecipientId = 15469717,
                        RecipientName = "starix2",
                        RecipientType = (int)TicketRecipient.USER
                    }
                ],
                Subtypes = [new Subtype { TypeId = 1, SubtypeId = 1, SubtypeName = "Verify League" }]
            },
            new()
            {
                TicketType = new TicketType
                {
                    TypeId = 2,
                    TypeName = "type2"
                },
                Recipients =
                [
                    new Recipient
                    {
                        TicketTypeRecipientId = 5,
                        RecipientId = 2,
                        RecipientName = "CSD",
                        RecipientType = (int)TicketRecipient.GROUP
                    }
                ],
                Subtypes = [new Subtype { TypeId = 2, SubtypeId = 5, SubtypeName = "Check League" }]
            },
            new()
            {
                TicketType = new TicketType
                {
                    TypeId = 3,
                    TypeName = "type3"
                },
                Recipients = [],
                Subtypes = []
            }
        };

        var actual = await handler.Handle(new GetTicketTypeRecipientRequest(), new CancellationToken(false));

        DeepAssert.Equal(expected, actual.ToList());
    }

    private GetTicketTypeRecipientHandler MockHandler()
    {
        var mockTicketTypes = new List<TicketType>
        {
            new()
            {
                TypeId = 1,
                TypeName = "type1"
            },
            new()
            {
                TypeId = 2,
                TypeName = "type2"
            },
            new()
            {
                TypeId = 3,
                TypeName = "type3"
            }
        };

        var mockRecipients = new List<GetTicketTypeRecipientHandler.RecipientData>
        {
            new()
            {
                TypeId = 1,
                TicketTypeRecipientId = 1,
                RecipientId = 1070340,
                RecipientName = "starix1",
                RecipientType = (int)TicketRecipient.USER
            },
            new()
            {
                TypeId = 1,
                TicketTypeRecipientId = 2,
                RecipientId = 1,
                RecipientName = "LA",
                RecipientType = (int)TicketRecipient.GROUP
            },
            new()
            {
                TypeId = 1,
                TicketTypeRecipientId = 3,
                RecipientId = 15469717,
                RecipientName = "starix2",
                RecipientType = (int)TicketRecipient.USER
            },
            new()
            {
                TypeId = 1,
                TicketTypeRecipientId = 4,
                RecipientId = 2,
                RecipientName = "CSD",
                RecipientType = (int)TicketRecipient.GROUP
            },
            new()
            {
                TypeId = 2,
                TicketTypeRecipientId = 5,
                RecipientId = 2,
                RecipientName = "CSD",
                RecipientType = (int)TicketRecipient.GROUP
            }
        };

        var mockSubtype = new List<Subtype>
        {
            new() { TypeId = 1, SubtypeId = 1, SubtypeName = "Verify League" },
            new() { TypeId = 2, SubtypeId = 5, SubtypeName = "Check League" }
        };

        var dynamicRepo = Substitute.For<IDynamicRepository>();
        dynamicRepo.FetchMultipleAsync<TicketType, GetTicketTypeRecipientHandler.RecipientData, Subtype>(Arg.Any<GetTicketTypeRecipientHandler.GetRecipientDataCriteria>())
            .Returns((mockTicketTypes, mockRecipients, mockSubtype));

        return new GetTicketTypeRecipientHandler(dynamicRepo);
    }
}