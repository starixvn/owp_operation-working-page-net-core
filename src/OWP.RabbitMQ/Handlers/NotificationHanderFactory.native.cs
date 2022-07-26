using MediatR;
using OWP.Models;
using OWP.NotificationRequests;
using OWP.RabbitMQ.Constants;

namespace OWP.RabbitMQ.Handler;

public interface NotificationHanderFactory
{
    Task Handle(string key, string message);
}

public class NotificationHanderFactoryImp : NotificationHanderFactory
{
    private readonly IMediator mediator;

    public NotificationHanderFactoryImp(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task Handle(string key, string message)
    {
        var request = CreateRequest(key, message);
        await mediator.Send(request);
    }

    private static NotificationCommandBaseModel CreateRequest(string key, string message)
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
