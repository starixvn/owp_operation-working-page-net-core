using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers
{
    public class DeleteRecipientRequest: RecipientRequestBase
    {
       
    }

    public class DeleteRecipientHandler: IRequestHandler<DeleteRecipientRequest, bool>
    {
        private readonly InsertOrUpdateRecipient recipientService;

        public DeleteRecipientHandler(InsertOrUpdateRecipient recipientService)
        {
            this.recipientService = recipientService;
        }

        public async Task<bool> Handle(DeleteRecipientRequest request, CancellationToken cancellationToken)
            => await recipientService.Execute(request.Recipients, request.TicketId, RecipientAction.Delete);
    }
}
