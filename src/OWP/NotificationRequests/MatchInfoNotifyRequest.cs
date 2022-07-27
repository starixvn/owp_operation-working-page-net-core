using MediatR;
using OWP.Models;

namespace OWP.NotificationRequests;

public class MatchInfoNotifyRequest : NotificationCommandBaseModel, IRequest<Unit>
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }
}