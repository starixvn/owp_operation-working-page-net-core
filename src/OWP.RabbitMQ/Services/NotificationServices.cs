using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using OWP.Hubs;
using OWP.Models;
using OWP.RabbitMQ.Constants;
using OWP.RabbitMQ.Model;

namespace OWP.RabbitMQ.Services;

public interface NotificationServices
{
    Task NotifyByRequest(string key, string message);
}

public class NotificationServicesImp : NotificationServices
{
    private readonly IHubContext<ChangedInfoNotifyHubImp, ChangedInfoNotifyHub> notifyChangedHub;

    public NotificationServicesImp(IHubContext<ChangedInfoNotifyHubImp, ChangedInfoNotifyHub> notifyChangedHub)
    {
        this.notifyChangedHub = notifyChangedHub;
    }

    public async Task NotifyByRequest(string key, string message)
    {
        switch (key)
        {
            case RoutingKey.OtherSport:
            case RoutingKey.InfositeMatchInfo:
            case RoutingKey.LinkedLgaMatch:
            case RoutingKey.PrematchNewLeague:
                await NotifyBase(message);
                break;
            case RoutingKey.TicketCenterNewMessage:
            case RoutingKey.TicketCenterNewRecipient:
            case RoutingKey.TicketCenterNewTicketEvent:
            case RoutingKey.TicketCenterNewTicketLeague:
            case RoutingKey.TicketCenterNewTicketOther:
            case RoutingKey.TicketCenterNewTicketSupport:
                await NotifyForTicketCenter(message); // need to review
                break;
        }
    }

    public async Task NotifyBase(string messageInString)
    {
        var messageModels = JsonConvert.DeserializeObject<List<MessageModel>>(messageInString);
        foreach (var message in messageModels)
        {
            var result = new
            {
                notificationId = Guid.NewGuid(),
                message
            };
            await notifyChangedHub.Clients
                .All
                //.Users(message.ReceivedUserIds)
                .notifyChangedInformationData(result);
        }
    }

    public async Task NotifyForTicketCenter(string messageInString)
    {
        var messageModels = JsonConvert.DeserializeObject<List<TicketMessageModel>>(messageInString);
        foreach (var message in messageModels)
        {
            var result = new
            {
                notificationId = Guid.NewGuid(),
                message
            };
            await notifyChangedHub.Clients
                //.Users($"{content.Message.UserId}")
                .All
                .notifyChangedInformationData(result);
        }
    }
}