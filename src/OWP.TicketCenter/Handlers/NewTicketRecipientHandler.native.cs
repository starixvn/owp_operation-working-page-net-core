using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers
{
    public class NewTicketRecipientRequest : RecipientRequestModelBase
    {
    }

    public class NewTicketRecipientHandler : IRequestHandler<NewTicketRecipientRequest, bool>
    {
        private readonly InsertOrUpdateRecipientService recipientService;

        public NewTicketRecipientHandler(InsertOrUpdateRecipientService recipientService)
        {
            this.recipientService = recipientService;
        }

        public async Task<bool> Handle(NewTicketRecipientRequest request, CancellationToken cancellationToken)
            => await recipientService.Execute(request.Recipients, request.TicketId, RecipientAction.Add);

    }
}
