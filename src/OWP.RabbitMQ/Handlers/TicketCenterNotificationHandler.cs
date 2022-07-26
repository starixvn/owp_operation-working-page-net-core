using MediatR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using OWP.Hubs;

namespace OWP.RabbitMQ.Handlers;

public class TicketCenterNotifyRequest : IRequest<Unit>
{
    public string Message { get; set; }
}



public class TicketCenterNotificationHandler : IRequestHandler<TicketCenterNotifyRequest, Unit>
{
    private readonly IHubContext<ChangedInfoNotifyHubImp, ChangedInfoNotifyHub> notifyChangedHub;

    public TicketCenterNotificationHandler(IHubContext<ChangedInfoNotifyHubImp, ChangedInfoNotifyHub> notifyChangedHub)
    {
        this.notifyChangedHub = notifyChangedHub;
    }

    public async Task<Unit> Handle(TicketCenterNotifyRequest request, CancellationToken cancellationToken)
    {
        var contents = JsonConvert.DeserializeObject<List<TicketCenterContent>>(request.Message);

        foreach (var content in contents)
        {
            await notifyChangedHub.Clients
                //.Users($"{content.Message.UserId}")
                .All
                .notifyChangedInformationData(content);
        }

        return Unit.Value;
    }
}


public class MessageModel
{
    public int UserId { get; set; }

    public bool IsImportant { get; set; }

    public int MessageType { get; set; }

    public string MessageContent { get; set; }
}


public class NotificationModel : MessageModel
{
    public int TicketId { get; set; }

    public int TicketType { get; set; }

    public int EventType { get; set; }

    public int? TotalMessageUnread { get; set; }

    public bool IsNewMessage { get; set; }
}


public class TicketCenterContent
{
    public string NotificationId { get; set; }
    public NotificationModel Message { get; set; }
}
