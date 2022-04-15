using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using OWP.RabbitMQ.Constants;

namespace OWP.RabbitMQ;

public abstract class RabbitMqClient : IDisposable
{
  protected readonly IModel channel;
  private readonly IConnection connection;
  private readonly IConnectionFactory connectionFactory;
  private readonly IConfiguration configuration;

  public RabbitMqClient(
      ConnectionFactory connectionFactory,
      IConfiguration configuration)
  {
    this.connectionFactory = connectionFactory;
    this.configuration = configuration;
    connection = connectionFactory.CreateConnection();
    channel = connection.CreateModel();
  }

  protected void ConnectRabbitMQ()
  {
    var exchange = configuration.GetSection("RabbitMQ:Exchange").Value;
    var queue = configuration.GetSection("RabbitMQ:Queue").Value;

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