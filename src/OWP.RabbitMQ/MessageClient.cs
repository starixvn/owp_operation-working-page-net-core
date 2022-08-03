using Fanex.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OWP.RabbitMQ.Model;
using RabbitMQ.Client;
using System.Text;

namespace OWP.RabbitMQ;

public class MessageClient
{
    private IBasicProperties props;
    private IModel channel;

    private readonly RabbitMQSetting setting;
    public readonly ILogger logger;

    public MessageClient(
        IOptions<RabbitMQSetting> options,
        ILogger logger)
    {
        this.setting = options.Value;
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

            channel.QueueDeclare(queue: setting.RequestQueue, durable: true, exclusive: false, autoDelete: false);
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

        channel.BasicPublish(
            exchange: string.Empty,
            routingKey: setting.RequestQueue,
            basicProperties: props,
            body: messageBytes);
    }
}