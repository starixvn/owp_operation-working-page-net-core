using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace OWP.MessagePublisher;


public interface IMessagePublisher
{
    void PushMessage<T>(T message, string routingKey);
}

public class Publisher : IMessagePublisher
{
    private readonly RabbitMQSetting setting;
    public Publisher(IOptions<RabbitMQSetting> options)
    {
        setting = options.Value;
    }
    public void PushMessage<T>(T message, string routingKey)
    {
        var factory = new ConnectionFactory
        {
            HostName = setting.Hosting,
            VirtualHost = setting.VirtualHost,
            Port = setting.Port,
            Password = setting.Password,
            UserName = setting.UserName
        };

        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: setting.Exchange, type: "direct", durable: true, autoDelete: false);
        channel.QueueDeclare(queue: setting.Queue, durable: true, exclusive: false, autoDelete: false);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: setting.Exchange, routingKey: routingKey, body: body);
    }
}
