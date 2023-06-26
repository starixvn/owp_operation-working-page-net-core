using MediatR;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers;

public class RemoveSingleRecipientRequest : InsertOrRemoveSingleRecipientModel, IRequest<int>
{
}

public class RemoveSingleRecipientHandler : IRequestHandler<RemoveSingleRecipientRequest, int>
{
    private readonly RecipientService recipientService;
    private readonly Repository repository;

    private readonly PreMatchNotificationService preMatchNotification;

    public RemoveSingleRecipientHandler(Repository repository, RecipientService recipientService, PreMatchNotificationService preMatchNotification)
    {
        this.repository = repository;
        this.recipientService = recipientService;
        this.preMatchNotification = preMatchNotification;
    }

    public async Task<int> Handle(RemoveSingleRecipientRequest request, CancellationToken cancellationToken)
    {
        var recipients = new List<InsertOrRemoveManyRecipientModel>
        {
            new()
            {
                RecipientIds = new List<int> { request.RecipientId },
                RecipientType = request.RecipientType
            }
        };
        await recipientService.RemoveManyRecipientReader(recipients, request.TicketId);
        var result = repository.RemoveSingleRecipient(request);
        await preMatchNotification.NotifyToReloadTicketInPrematchTable(request.TicketId, request.TicketEventType, new List<string> { request.RecipientId.ToString() });

        return await result;
    }
}