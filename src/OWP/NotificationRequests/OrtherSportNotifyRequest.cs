using MediatR;
using OWP.Models;

namespace OWP.NotificationRequests;

public class OtherSportNotifyRequest : NotificationCommandBaseModel, IRequest<Unit>
{
    public string Message { get; set; }
}

