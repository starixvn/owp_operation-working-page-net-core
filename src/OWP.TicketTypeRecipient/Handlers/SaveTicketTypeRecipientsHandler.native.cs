namespace OWP.TicketTypeRecipient.Handlers
{
    using Fanex.Data.Repository;
    using MediatR;
    using Olympus.Xml;
    using OWP.TicketTypeRecipient.Models;

    public class SaveTicketTypeRecipientsCommand : IRequest<Unit>
    {
        public int TicketTypeId { get; set; }

        public List<Recipient> Recipients { get; set; }

        public List<Subtype> Subtypes { get; set; }
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
                Recipients = XmlConverter.Serialize(request.Recipients),
                Subtypes = XmlConverter.Serialize(request.Subtypes)
            };
            await repository.ExecuteAsync(criteria);

            return await Task.FromResult(Unit.Value);
        }

        public class SaveRecipientsCriteria : NonQueryCommand
        {
            public int TicketTypeId { get; set; }

            public string Recipients { get; set; }

            public string Subtypes { get; set; }

            public override string GetSettingKey() => "OWP_TicketTypeRecipient_SaveTicketTypeRecipient";

            public override bool IsValid() => true;
        }
    }
}