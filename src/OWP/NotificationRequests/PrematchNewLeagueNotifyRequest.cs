using MediatR;
using OWP.Models;

namespace OWP.NotificationRequests;

public class PrematchNewLeagueNotifyRequest : NotificationCommandBaseModel, IRequest<Unit>
{
    public DateTime FromDate { get; set; }
}