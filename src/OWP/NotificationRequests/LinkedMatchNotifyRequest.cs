using MediatR;
using OWP.Models;

namespace OWP.NotificationRequests;

public class LinkedMatchNotifyRequest : NotificationCommandBaseModel, IRequest<Unit>
{
  public DateTime EventDate { get; set; }
}
