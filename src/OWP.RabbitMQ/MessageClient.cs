using Fanex.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OWP.RabbitMQ.Handler;
using OWP.RabbitMQ.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace OWP.RabbitMQ;

public class MessageClient
{
    private IBasicProperties props;
    private IModel channel;
    private AsyncEventingBasicConsumer consumer;

    private readonly RabbitMQSetting setting;
    private readonly NotificationHanderFactory notificationHander;
    public readonly ILogger logger;

    public MessageClient(
        IOptions<RabbitMQSetting> options,
        NotificationHanderFactory notificationHander,
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
            var factory = new ConnectionFactory()
            {
                UserName = setting.UserName,
                Password = setting.Password,
                HostName = setting.Hosting,
                Port = setting.Port,
                VirtualHost = setting.VirtualHost,
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(queue: setting.ReplyQueue, durable: true, exclusive: false, autoDelete: false);

            consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += OnReceived;

            channel.BasicConsume(consumer: consumer, queue: setting.ReplyQueue, autoAck: true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
        }
    }

    public void SendMessage(object message, string messageType)
    {
        var json = JsonConvert.SerializeObject(message);
        var messageBytes = Encoding.UTF8.GetBytes(json);

        props = channel.CreateBasicProperties();
        props.Type = messageType;
        props.ReplyTo = setting.ReplyQueue;

        channel.BasicPublish(
            exchange: string.Empty,
            routingKey: setting.RequestQueue,
            basicProperties: props,
            body: messageBytes);
    }

    private async Task OnReceived(object model, BasicDeliverEventArgs eventArgs)
    {
        var body = eventArgs.Body.ToArray();
        var props = eventArgs.BasicProperties;
        var response = Encoding.UTF8.GetString(body);

        await notificationHander.Handle(props.Type, response);
    }
}