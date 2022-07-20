using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OWP.Hubs;
using OWP.MessagePublisher;
using OWP.NotificationHandler.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace OWP.NotificationHandler;


public class MessageClient
{
    private const string QueueName = "rpc_queue";

    private IBasicProperties props;

    private readonly RabbitMQSetting setting;
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly EventingBasicConsumer consumer;
    private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();
    private readonly IHubContext<ChangedInfoNotifyHubImp, ChangedInfoNotifyHub> notifyChangedHub;

    public MessageClient(IOptions<RabbitMQSetting> options, IHubContext<ChangedInfoNotifyHubImp, ChangedInfoNotifyHub> notifyChangedHub)
    {
        this.setting = options.Value;
        this.notifyChangedHub = notifyChangedHub;

        var factory = new ConnectionFactory() { HostName = setting.Hosting };

        connection = factory.CreateConnection();
        channel = connection.CreateModel();

        channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        consumer = new EventingBasicConsumer(channel);

        consumer.Received += OnReceived;

        channel.BasicConsume(consumer: consumer, queue: QueueName, autoAck: true);

    }

    public void SendMessage(object message, string correlationId)
    {
        var json = JsonConvert.SerializeObject(message);
        var messageBytes = Encoding.UTF8.GetBytes(json);

        props = channel.CreateBasicProperties();
        props.CorrelationId = correlationId;
        props.ReplyTo = QueueName;

        channel.BasicPublish(
            exchange: "",
            routingKey: "Message",
            basicProperties: props,
            body: messageBytes);
    }

    private async void OnReceived(object model, BasicDeliverEventArgs eventArgs)
    {
        var body = eventArgs.Body.ToArray();
        var props = eventArgs.BasicProperties;
        var response = Encoding.UTF8.GetString(body);

        await HandleSendNotification(props.CorrelationId, response);
    }

    private async Task HandleSendNotification(string key, string message)
    {
        // ticket center
        if (key == RoutingKey.TicketCenterNewRecipient
            || key == RoutingKey.TicketCenterNewRecipient
            || key == RoutingKey.TicketCenterNewTicketEvent
            || key == RoutingKey.TicketCenterNewTicketLeague
            || key == RoutingKey.TicketCenterNewTicketOther
            || key == RoutingKey.TicketCenterNewTicketSupport)
        {
            await HandleSendNotificationTicketCenter(message);
            return;
        }

        // other sport
        if (key == RoutingKey.OtherSport)
        {
            await HandleSendNotificationOtherSport(message);
            return;
        }
    }

    private async Task HandleSendNotificationTicketCenter(string message)
    {
        var lists = JsonConvert.DeserializeObject<List<TicketCenterContent>>(message);

        foreach (var item in lists)
        {
            await notifyChangedHub.Clients
            //.Users(item.userId)
            .All
            .notifyChangedInformationData(item);
        }
    }

    private async Task HandleSendNotificationOtherSport(string message)
    {
        var lists = JsonConvert.DeserializeObject<List<OtherSportContent>>(message);

        foreach (var item in lists)
        {
            await notifyChangedHub.Clients
            //.Users(item.Message.ReceivedUserIds)
            .All
            .notifyChangedInformationData(item);
        }
    }

    public void Close()
    {
        connection.Close();
    }
}


