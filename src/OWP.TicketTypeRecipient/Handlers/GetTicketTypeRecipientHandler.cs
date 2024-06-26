using Fanex.Data.Repository;
using MediatR;
using OWP.TicketTypeRecipient.Models;

namespace OWP.TicketTypeRecipient.Handlers;

public class GetTicketTypeRecipientRequest : IRequest<IEnumerable<Models.TicketTypeRecipient>>
{
}

public class GetTicketTypeRecipientHandler : IRequestHandler<GetTicketTypeRecipientRequest, IEnumerable<Models.TicketTypeRecipient>>
{
    private readonly IDynamicRepository repository;

    public GetTicketTypeRecipientHandler(IDynamicRepository dynamicRepository)
    {
        repository = dynamicRepository;
    }

    public async Task<IEnumerable<Models.TicketTypeRecipient>> Handle(GetTicketTypeRecipientRequest request, CancellationToken cancellationToken)
    {
        var (ticketTypes, recipients, subtypes) = await repository.FetchMultipleAsync<TicketType, RecipientData, Subtype>(new GetRecipientDataCriteria());

        return GetResult(ticketTypes, recipients.ToList(), subtypes.ToList());
    }

    private static IEnumerable<Models.TicketTypeRecipient> GetResult(IEnumerable<TicketType> ticketTypes, List<RecipientData> recipients, List<Subtype> subtypes)
    {
        var sortedTicketTypes = ticketTypes.OrderBy(x => x.TypeName);

        foreach (var type in sortedTicketTypes)
        {
            yield return new Models.TicketTypeRecipient
            {
                TicketType = type,
                Recipients = recipients.Where(x => x.TypeId == type.TypeId)
                    .Select(y => new Recipient
                    {
                        RecipientId = y.RecipientId,
                        RecipientName = y.RecipientName,
                        RecipientType = y.RecipientType,
                        TicketTypeRecipientId = y.TicketTypeRecipientId,
                        SubtypeName = y.SubtypeName
                    })
                    .OrderBy(x => x.RecipientName)
                    .ToList(),
                Subtypes = subtypes
                    .Where(x => x.TypeId == type.TypeId)
                    .OrderBy(x => x.SubtypeName)
                    .ToList()
            };
        }
    }

    public class GetRecipientDataCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "OWP_TicketTypeRecipient_GetTicketTypeRecipient";

        public override bool IsValid() => true;
    }

    public class RecipientData
    {
        public int TypeId { get; set; }

        public int TicketTypeRecipientId { get; set; }

        public int RecipientId { get; set; }

        public int RecipientType { get; set; }

        public string RecipientName { get; set; }

        public string SubtypeName { get; set; }
    }
}