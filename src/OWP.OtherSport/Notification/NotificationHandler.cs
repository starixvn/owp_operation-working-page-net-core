using MediatR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using OWP.Hubs;
using OWP.NotificationRequests;

namespace OWP.OtherSport.Notification
{
    internal class NotificationHandler : IRequestHandler<OtherSportNotifyRequest, Unit>
    {
        private readonly IHubContext<ChangedInfoNotifyHubImp, ChangedInfoNotifyHub> notifyChangedHub;

        public NotificationHandler(IHubContext<ChangedInfoNotifyHubImp, ChangedInfoNotifyHub> notifyChangedHub)
        {
            this.notifyChangedHub = notifyChangedHub;
        }

        public async Task<Unit> Handle(OtherSportNotifyRequest request, CancellationToken cancellationToken)
        {
            var contents = JsonConvert.DeserializeObject<List<OtherSportContent>>(request.Message);

            foreach (var content in contents)
            {
                await notifyChangedHub.Clients
                    //.Users($"{content.Message.ReceivedUserIds}")
                    .All
                    .notifyChangedInformationData(content);
            }

            return Unit.Value;
        }
    }
}


public class OtherSportMessage
{
    public bool IsImportant { get; set; }

    public int MessageType { get; set; }

    public string MessageContent { get; set; }

    public List<string> ReceivedUserIds { get; set; }

    public string ReceivedUserIdList { get; set; }

    public bool IsWatcher { get; set; }
}
public class OtherSportContent
{
    public string NotificationId { get; set; }
    public OtherSportMessage Message { get; set; }
}
