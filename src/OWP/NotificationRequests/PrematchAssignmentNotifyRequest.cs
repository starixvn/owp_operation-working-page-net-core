using System.Globalization;
using MediatR;
using OWP.Models;

namespace OWP.NotificationRequests;

public class PrematchAssignmentNotifyRequest : NotificationCommandBaseModel, IRequest<Unit>
{
    public string RedisKey { get; set; }

    public PrematchAssignmentNotifyRequest()
    {
        SessionId = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
    }
}