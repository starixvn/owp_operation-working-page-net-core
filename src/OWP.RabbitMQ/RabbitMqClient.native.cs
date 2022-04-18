using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using OWP.RabbitMQ.Constants;

namespace OWP.RabbitMQ;

public abstract class RabbitMqClient : IDisposable
{
#pragma warning disable CS0649 // Field 'RabbitMqClientImp.channel/connection' is never assigned to
  protected IModel channel;
  private IConnection connection;
#pragma warning restore CS0649
  private readonly IConnectionFactory connectionFactory;
  private readonly IConfiguration configuration;

  public RabbitMqClient(IConfiguration configuration)
  {
    this.configuration = configuration;

    var hosting = configuration.GetSection("RabbitMQ:Hosting").Value;
    var virtualHosting = configuration.GetSection("RabbitMQ:VirtualHost").Value;
    var username = configuration.GetSection("RabbitMQ:UserName").Value;
    var password = configuration.GetSection("RabbitMQ:Password").Value;
    var port = int.Parse(configuration.GetSection("RabbitMQ:Port").Value);

    connectionFactory = new ConnectionFactory
    {
      UserName = username,
      Password = password,
      HostName = hosting,
      Port = port,
      VirtualHost = virtualHosting,
      DispatchConsumersAsync = true
    };
  }

  protected void ConnectRabbitMQ()
  {
    var exchange = configuration.GetSection("RabbitMQ:Exchange").Value;
    var queue = configuration.GetSection("RabbitMQ:Queue").Value;

    connection = connectionFactory.CreateConnection();
    channel = connection.CreateModel();

    channel.ExchangeDeclare(
        exchange: exchange,
        type: "direct",
        durable: true,
        autoDelete: false);

    channel.QueueDeclare(
        queue: queue,
        durable: true,
        exclusive: false,
        autoDelete: false);

    foreach (var key in RoutingKey.Keys)
    {
      channel.QueueBind(
      queue: queue,
      exchange: exchange,
      routingKey: key);
    }
  }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool isDisposing)
  {
    if (channel != null && channel.IsOpen)
    {
      channel?.Close();
      channel?.Dispose();
    }

    if (connection != null && connection.IsOpen)
    {
      connection?.Close();
      connection?.Dispose();
    }
  }
}