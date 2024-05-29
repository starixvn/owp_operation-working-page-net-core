using MediatR;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;

namespace OWP.TicketCenter.Handlers;

public class UpdateMessageIconRequest : MessageIconModel, IRequest { }

public class UpdateMessageIconHandler : IRequestHandler<UpdateMessageIconRequest>
{
    private readonly Repository repository;
    private readonly NotificationService notification;

    public UpdateMessageIconHandler(Repository repository, NotificationService notification)
    {
        this.repository = repository;
        this.notification = notification;
    }

    public async Task Handle(UpdateMessageIconRequest request, CancellationToken cancellationToken)
    {
        var criteria = new UpdateMessageIconCriteria
        {
            CreatorId = request.CreatorId,
            IconKey = request.IconKey,
            MessageId = request.MessageId
        };
        var action = await repository.UpdateMessageIcon(criteria);
        var messageReactIcon = (await repository.GetMessageReactIcon(request.MessageId.ToString())).FirstOrDefault();
        messageReactIcon!.IconKey = request.IconKey;
        var userIds = await repository.GetUserIdsByTicketId(request.TicketId);

        await notification.NotifyUpdateReactIconMessage(userIds, messageReactIcon, action, request.TicketId);
    }
}