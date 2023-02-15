namespace OWP.TicketTypeRecipient.Handlers;

using Fanex.Data.Repository;
using MediatR;
using Models;

public class GetTicketTypeRecipientRequest : IRequest<IEnumerable<TicketTypeRecipient>>
{
}

public class GetTicketTypeRecipientHandler : IRequestHandler<GetTicketTypeRecipientRequest, IEnumerable<TicketTypeRecipient>>
{
    private readonly IDynamicRepository repository;

    public GetTicketTypeRecipientHandler(IDynamicRepository dynamicRepository)
    {
        repository = dynamicRepository;
    }

    public async Task<IEnumerable<TicketTypeRecipient>> Handle(GetTicketTypeRecipientRequest request, CancellationToken cancellationToken)
    {
        var (ticketTypes, recipients, subtypes) = await repository.FetchMultipleAsync<TicketType, RecipientData, Subtype>(new GetRecipientDataCriteria());

        return GetResult(ticketTypes, recipients, subtypes);
    }

    private static IEnumerable<TicketTypeRecipient> GetResult(IEnumerable<TicketType> ticketTypes, IEnumerable<RecipientData> recipients, IEnumerable<Subtype> subtypes)
    {
        var sortedTicketTypes = ticketTypes.OrderBy(x => x.TypeName);

        foreach (var type in sortedTicketTypes)
        {
            yield return new TicketTypeRecipient
            {
                TicketType = type,
                Recipients = recipients.Where(x => x.TypeId == type.TypeId)
                    .Select(y => new Recipient
                    {
                        RecipientId = y.RecipientId,
                        RecipientName = y.RecipientName,
                        RecipientType = y.RecipientType,
                        TicketTypeRecipientId = y.TicketTypeRecipientId
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
    }
}