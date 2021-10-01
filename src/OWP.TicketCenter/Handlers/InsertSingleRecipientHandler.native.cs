using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers
{
    public class InsertSingleRecipientRequest: InsertOrRemoveSingleRecipientModel, IRequest<int>
    {
    }

    public class InsertRecipientHandler: IRequestHandler<InsertSingleRecipientRequest, int>
    {
        private readonly Repository repository;

        public InsertRecipientHandler(IMapper mapper, Repository repository)
        {
            this.repository = repository;
        }
        public async Task<int> Handle(InsertSingleRecipientRequest request, CancellationToken cancellationToken)
        {
            return await repository.AddSingleRecipient(request);
        }
    }
}
