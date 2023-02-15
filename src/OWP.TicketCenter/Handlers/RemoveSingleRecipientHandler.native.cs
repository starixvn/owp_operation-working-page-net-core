namespace OWP.TicketCenter.Handlers;

using MediatR;
using Models;
using Services;

public class RemoveSingleRecipientRequest : InsertOrRemoveSingleRecipientModel, IRequest<int>
{
}

public class RemoveSingleRecipientHandler : IRequestHandler<RemoveSingleRecipientRequest, int>
{
    private readonly Repository repository;

    private readonly RecipientService recipientService;

    public RemoveSingleRecipientHandler(Repository repository, RecipientService recipientService)
    {
        this.repository = repository;
        this.recipientService = recipientService;
    }

    public async Task<int> Handle(RemoveSingleRecipientRequest request, CancellationToken cancellationToken)
    {
        var recipients = new List<InsertOrRemoveManyRecipientModel>
        {
            new ()
            {
                RecipientIds = new List<int> {request.RecipientId},
                RecipientType = request.RecipientType
            }
        };
        await recipientService.RemoveManyRecipientReader(recipients, request.TicketId);
        return await repository.RemoveSingleRecipient(request);
    }
}