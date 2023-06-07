using MediatR;
using OWP.RabbitMQ;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers;

public class NewTicketIssueLogWorkRequest : NewTicketModelBase, IRequest<int>
{
    public int AdminSiteId { get; set; }

    public string LeagueName { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public string Subject { get; set; }

    public DateTime EventDateTime { get; set; }
}

public class NewTicketIssueLogWorkHandler : IRequestHandler<NewTicketIssueLogWorkRequest, int>
{
    private readonly MessageClient messageClient;
    private readonly MessageService messageService;
    private readonly NotificationService notificationService;
    private readonly RecipientService recipientService;
    private readonly Repository repository;

    public NewTicketIssueLogWorkHandler(
        Repository repository,
        MessageService messageService,
        NotificationService notificationService,
        RecipientService recipientService,
        MessageClient messageClient)
    {
        this.repository = repository;
        this.notificationService = notificationService;
        this.recipientService = recipientService;
        this.messageClient = messageClient;
        this.messageService = messageService;
    }

    public async Task<int> Handle(NewTicketIssueLogWorkRequest request, CancellationToken cancellationToken)
    {
        var ticketId = await repository.BuildTicketId(request.TicketType, request.SubType);

        request.TicketId = ticketId;

        await repository.AddNewTicketIssueLogWork(request);

        var criteria = new InsertIssueCreatorCriteria
        {
            TicketId = ticketId,
            CreatorId = request.CreatorId
        };

        await repository.InsertIssueCreator(criteria);

        await recipientService.InsertManyRecipient(request.Recipients, request.TicketId);

        await recipientService.InsertManyRecipientReader(request.Recipients, request.TicketId, request.CreatorId);

        // Notify --> Liam

        await notificationService.NotifyNewMessageIncome(ticketId);

        await notificationService.NotifyNewAddRecipient(ticketId, request.CreatorId);

        return ticketId;
    }
}