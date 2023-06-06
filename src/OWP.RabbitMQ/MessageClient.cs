using Fanex.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OWP.RabbitMQ.Model;
using RabbitMQ.Client;
using System.Text;

namespace OWP.RabbitMQ;

public interface MessageClient
{
    void Connect();
    void SendMessage(object message, string messageType);
}

public class MessageClientImp : MessageClient
{
    private IModel channel;

    private readonly RabbitMQSetting setting;
    private readonly ILogger logger;

    public MessageClientImp(
        IOptions<RabbitMQSetting> options,
        ILogger logger)
    {
        setting = options.Value;
        this.logger = logger;
    }

    public void Connect()
    {
        try
        {
            var factory = new ConnectionFactory
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

            channel.QueueDeclare(setting.RequestQueue, true, false, false);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
        }
    }

    public void SendMessage(object message, string messageType)
    {
        const int EXPIRATION_TIME = 1000 * 60 * 30;
        var json = JsonConvert.SerializeObject(message);
        var messageBytes = Encoding.UTF8.GetBytes(json);

        var props = channel.CreateBasicProperties();
        props.Type = messageType;
        props.Expiration = EXPIRATION_TIME.ToString();

        channel.BasicPublish(
            string.Empty,
            setting.RequestQueue,
            props,
            messageBytes);
    }
}