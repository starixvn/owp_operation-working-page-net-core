namespace OWP.TicketTypeRecipient.Handlers
{
    using System.Collections.Generic;
    using MediatR;
    using Fanex.Data.Repository;
    using OWP.TicketTypeRecipient.Models;
    using System.Threading.Tasks;
    using System.Threading;
    using Olympus.Xml;

    public class SaveTicketTypeRecipientsCommand : IRequest<Unit>
    {
        public int TicketTypeId { get; set; }

        public List<Recipient> Recipients { get; set; }
    }

    public class SaveTicketTypeRecipientsHandler : IRequestHandler<SaveTicketTypeRecipientsCommand>
    {
        private readonly IDynamicRepository repository;

        public SaveTicketTypeRecipientsHandler(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public async Task<Unit> Handle(SaveTicketTypeRecipientsCommand request, CancellationToken cancellationToken)
        {
            var criteria = new SaveRecipientsCriteria
            {
                TicketTypeId = request.TicketTypeId,
                Recipients = XmlConverter.Serialize(request.Recipients)
            };
            await repository.ExecuteAsync(criteria);

            return await Task.FromResult(Unit.Value);
        }

        public class SaveRecipientsCriteria : NonQueryCommand
        {
            public int TicketTypeId { get; set; }

            public string Recipients { get; set; }

            public override string GetSettingKey() => "OWP_TicketTypeRecipient_SaveTicketTypeRecipient";

            public override bool IsValid() => true;
        }
    }
}