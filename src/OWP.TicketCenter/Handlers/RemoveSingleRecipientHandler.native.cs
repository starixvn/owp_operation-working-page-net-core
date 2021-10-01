using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers
{
    public class RemoveSingleRecipientRequest: InsertOrRemoveSingleRecipientModel, IRequest<int>
    {
    }

    public class RemoveSingleRecipientHandler : IRequestHandler<RemoveSingleRecipientRequest, int>
    {
        private readonly Repository repository;

        public RemoveSingleRecipientHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(RemoveSingleRecipientRequest request, CancellationToken cancellationToken)
            => await repository.RemoveSingleRecipient(request);
    }
}
