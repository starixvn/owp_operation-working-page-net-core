namespace OWP.TicketTypeRecipient.UnitTests.Handlers;

using Fanex.Data.Repository;
using NSubstitute;
using OWP.Enums;
using OWP.TicketTypeRecipient.Handlers;
using OWP.TicketTypeRecipient.Models;

using Xunit;
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
                    TypeName = "type1"
                },
                Recipients = new List<Recipient>
                {
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
                },
                Subtypes = new List<Subtype> {
                    new Subtype { TypeId = 1, SubtypeId = 1, SubtypeName = "Verify League" }
                }
            },
            new TicketTypeRecipient
            {
                TicketType = new TicketType
                {
                    TypeId = 2,
                    TypeName = "type2"
                },
                Recipients = new List<Recipient>
                {
                    new Recipient
                    {
                        TicketTypeRecipientId = 5,
                        RecipientId = 2,
                        RecipientName = "CSD",
                        RecipientType = (int)TicketRecipient.GROUP
                    }
                },
                Subtypes = new List<Subtype> {
                    new Subtype { TypeId = 2, SubtypeId = 5, SubtypeName = "Check League" }
                }
            },
            new TicketTypeRecipient
            {
                TicketType = new TicketType
                {
                    TypeId = 3,
                    TypeName = "type3"
                },
                Recipients = new List<Recipient>(),
                Subtypes = new List<Subtype>()
            }
        };

        var actual = await handler.Handle(new GetTicketTypeRecipientRequest(), new System.Threading.CancellationToken(false));

        DeepAssert.Equal(expected, actual.ToList());
    }

    private GetTicketTypeRecipientHandler MockHander()
    {
        var mockTicketTypes = new List<TicketType>
        {
            new TicketType
            {
                TypeId = 1,
                TypeName = "type1"
            },
            new TicketType
            {
                TypeId = 2,
                TypeName = "type2"
            },
            new TicketType
            {
                TypeId = 3,
                TypeName = "type3"
            }
        };

        var mockRecipients = new List<GetTicketTypeRecipientHandler.RecipientData>
        {
            new GetTicketTypeRecipientHandler.RecipientData
            {
                TypeId = 1,
                TicketTypeRecipientId = 1,
                RecipientId = 1070340,
                RecipientName = "starix1",
                RecipientType = (int)TicketRecipient.USER,
            },
            new GetTicketTypeRecipientHandler.RecipientData
            {
                TypeId = 1,
                TicketTypeRecipientId = 2,
                RecipientId = 1,
                RecipientName = "LA",
                RecipientType = (int)TicketRecipient.GROUP,
            },
            new GetTicketTypeRecipientHandler.RecipientData
            {
                TypeId = 1,
                TicketTypeRecipientId = 3,
                RecipientId = 15469717,
                RecipientName = "starix2",
                RecipientType = (int)TicketRecipient.USER,
            },
            new GetTicketTypeRecipientHandler.RecipientData
            {
                TypeId = 1,
                TicketTypeRecipientId = 4,
                RecipientId = 2,
                RecipientName = "CSD",
                RecipientType = (int)TicketRecipient.GROUP,
            },
            new GetTicketTypeRecipientHandler.RecipientData
            {
                TypeId = 2,
                TicketTypeRecipientId = 5,
                RecipientId = 2,
                RecipientName = "CSD",
                RecipientType = (int)TicketRecipient.GROUP,
            }
        };

        var mockSubtype = new List<Subtype>
        {
            new Subtype { TypeId = 1, SubtypeId = 1, SubtypeName = "Verify League" },
            new Subtype { TypeId = 2, SubtypeId = 5, SubtypeName = "Check League" }
        };

        var dynamicRepo = Substitute.For<IDynamicRepository>();
        dynamicRepo.FetchMultipleAsync<TicketType, GetTicketTypeRecipientHandler.RecipientData, Subtype>(Arg.Any<GetTicketTypeRecipientHandler.GetRecipientDataCriteria>())
            .Returns((mockTicketTypes, mockRecipients, mockSubtype));

        return new GetTicketTypeRecipientHandler(dynamicRepo);
    }
}