using MediatR;
using OWP.Models;

namespace OWP.NotificationRequests;

public class TicketCenterNotifyRequest : NotificationCommandBaseModel, IRequest<Unit>
{
    public string Message { get; set; }
}
