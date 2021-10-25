using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers
{
    public class InsertSingleRecipientRequest : InsertOrRemoveSingleRecipientModel, IRequest<int>
    {
    }

    public class InsertRecipientHandler : IRequestHandler<InsertSingleRecipientRequest, int>
    {
        private readonly Repository repository;

        private readonly RecipientService recipientService;

        public InsertRecipientHandler(Repository repository, RecipientService recipientService)
        {
            this.repository = repository;
            this.recipientService = recipientService;
        }
        public async Task<int> Handle(InsertSingleRecipientRequest request, CancellationToken cancellationToken)
        {
            var recipients = new List<InsertOrRemoveManyRecipientModel>()
            {
                new ()
                {
                   RecipientIds = new List<int>(){request.RecipientId},
                   RecipientType = request.RecipientType
                }
            };

            await recipientService.InsertManyRecipientReader(recipients, request.TicketId);
            return await repository.AddSingleRecipient(request);
        }
    }
}
