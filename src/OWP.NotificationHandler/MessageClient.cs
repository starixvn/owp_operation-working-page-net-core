using Fanex.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace OWP.NotificationHandler;


public class MessageClient
{
    private const string QueueName = "rpc_queue";

    private IBasicProperties props;
    private IConnection connection;
    private IModel channel;
    private EventingBasicConsumer consumer;

    private readonly RabbitMQSetting setting;
    private readonly SendNotificationHander notificationHander;
    public readonly ILogger logger;

    public MessageClient(
        IOptions<RabbitMQSetting> options,
        SendNotificationHander notificationHander,
        ILogger logger)
    {
        this.setting = options.Value;
        this.notificationHander = notificationHander;
        this.logger = logger;
    }

    public void Connect()
    {
        try
        {
            var factory = new ConnectionFactory() { HostName = setting.Hosting };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(queue: QueueName, durable: true, autoDelete: false);

            consumer = new EventingBasicConsumer(channel);

            consumer.Received += OnReceived;

            channel.BasicConsume(consumer: consumer, queue: QueueName, autoAck: true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            //throw;
        }

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

        await notificationHander.Handle(props.CorrelationId, response);
    }


    public void Close()
    {
        connection.Close();
    }
}


