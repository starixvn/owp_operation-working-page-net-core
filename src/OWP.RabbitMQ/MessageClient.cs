using Fanex.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OWP.RabbitMQ.Model;
using RabbitMQ.Client;
using System.Text;

namespace OWP.RabbitMQ;

public interface MessageClient
{
    Task<bool> Connect();
    ValueTask<bool> SendMessage(object message, string messageType);
}

public class MessageClientImp : MessageClient
{
    private IChannel channel;

    private readonly RabbitMQSetting setting;
    private readonly ILogger logger;

    public MessageClientImp(
        IOptions<RabbitMQSetting> options,
        ILogger logger)
    {
        setting = options.Value;
        this.logger = logger;
    }

    public async Task<bool> Connect()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                UserName = setting.UserName,
                Password = setting.Password,
                HostName = setting.Hosting,
                Port = setting.Port,
                VirtualHost = setting.VirtualHost
            };

            var connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(setting.RequestQueue, true, false, false);
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);
            return false;
        }

        return true;
    }

    public async ValueTask<bool> SendMessage(object message, string messageType)
    {
        const int EXPIRATION_TIME = 1000 * 60 * 30;
        var json = JsonConvert.SerializeObject(message);
        var messageBytes = Encoding.UTF8.GetBytes(json);

        var props = new BasicProperties
        {
            Type = messageType,
            Expiration = EXPIRATION_TIME.ToString()
        };

        await channel.BasicPublishAsync(
            string.Empty,
            setting.RequestQueue,
            basicProperties: props,
            body: messageBytes,
            mandatory: true);

        return true;
    }
}