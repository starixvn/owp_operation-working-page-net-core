namespace OWP.TicketTypeRecipient.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using MediatR;
    using Models;

    public class GetTicketTypeRecipientRequest : IRequest<IList<TicketTypeRecipient>>
    {
    }

    public class GetTicketTypeRecipientHandler : IRequestHandler<GetTicketTypeRecipientRequest, IList<TicketTypeRecipient>>
    {
        private readonly IDynamicRepository repository;

        public GetTicketTypeRecipientHandler(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public async Task<IList<TicketTypeRecipient>> Handle(GetTicketTypeRecipientRequest request, CancellationToken cancellationToken)
        {
            var recipientData = await repository.FetchAsync<RecipientData>(new GetRecipientDataCriteria());

            var result = recipientData.GroupBy(x => new TicketType { TypeId = x.TypeId, TypeName = x.TypeName },
                x => new
                {
                    x.RecipientId,
                    x.RecipientName,
                    x.RecipientType,
                    x.TicketTypeRecipientId
                },
                (key, values) => new TicketTypeRecipient
                {
                    TicketType = key,
                    Recipients = values.Select(y => new Recipient
                    {
                        RecipientId = y.RecipientId,
                        RecipientName = y.RecipientName,
                        RecipientType = y.RecipientType,
                        TicketTypeRecipientId = y.TicketTypeRecipientId
                    }).ToList()
                },
                new TicketType()).ToList();

            return result;
        }

        public class GetRecipientDataCriteria : CriteriaBase
        {
            public override string GetSettingKey() => "OWP_TicketTypeRecipient_GetTicketTypeRecipient";

            public override bool IsValid() => true;
        }

        public class RecipientData
        {
            public int TypeId { get; set; }

            public string TypeName { get; set; }

            public int TicketTypeRecipientId { get; set; }

            public int RecipientId { get; set; }

            public int RecipientType { get; set; }

            public string RecipientName { get; set; }
        }
    }
}