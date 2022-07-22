using MediatR;
using OWP.Models;
using OWP.RabbitMQ.Constants;
using OWP.NotificationRequests;

namespace OWP.RabbitMQ.Handler;

public interface SendNotificationHander
{
    Task Handle(string key, string message);
}

public class SendNotificationHanderImp : SendNotificationHander
{
    private readonly IMediator mediator;

    public SendNotificationHanderImp(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task Handle(string key, string message)
    {
        var request = CreateRequest(key, message);
        await mediator.Send(request);
    }

    private NotificationCommandBaseModel CreateRequest(string key, string message)
    {
        switch (key)
        {
            case RoutingKey.TicketCenterNewMessage:
            case RoutingKey.TicketCenterNewRecipient:
            case RoutingKey.TicketCenterNewTicketEvent:
            case RoutingKey.TicketCenterNewTicketLeague:
            case RoutingKey.TicketCenterNewTicketOther:
            case RoutingKey.TicketCenterNewTicketSupport:
                return new TicketCenterNotifyRequest { Message = message };
            case RoutingKey.OtherSport:
                return new OtherSportNotifyRequest { Message = message };
            default:
                return new NotificationCommandBaseModel();
        }
    }
}
