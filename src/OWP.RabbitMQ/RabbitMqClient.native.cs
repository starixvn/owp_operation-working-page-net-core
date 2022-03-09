using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using OWP.RabbitMQ.Constants;

namespace OWP.RabbitMQ;

public abstract class RabbitMqClient : IDisposable
{
  protected readonly IModel channel;
  private readonly IConnection connection;
  private readonly ConnectionFactory connectionFactory;
  private readonly IConfiguration configuration;

  public RabbitMqClient(
      ConnectionFactory connectionFactory,
      IConfiguration configuration)
  {
    this.connectionFactory = connectionFactory;
    this.configuration = configuration;

    var exchange = configuration.GetSection("RabbitMQ:Exchange").Value;
    var queue = configuration.GetSection("RabbitMQ:Queue").Value;

    if (connection == null || !connection.IsOpen)
    {
      connection = connectionFactory.CreateConnection();
    }

    if (channel == null || !channel.IsOpen)
    {
      channel = connection.CreateModel();
      channel.ExchangeDeclare(
          exchange: "NAP.OWP.Notification",
          type: "direct",
          durable: true,
          autoDelete: false);

      channel.QueueDeclare(
          queue: "NAP.OWP.Notification.message",
          durable: false,
          exclusive: false,
          autoDelete: false);

      foreach (var key in RoutingKey.Keys)
      {
        channel.QueueBind(
        queue: "NAP.OWP.Notification.message",
        exchange: "NAP.OWP.Notification",
        routingKey: key);
      }
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