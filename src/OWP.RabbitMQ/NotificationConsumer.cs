using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OWP.RabbitMQ;

public class NotificationConsumer : ConsumerBase, IHostedService
{
  public NotificationConsumer(IMediator mediator,
  ConnectionFactory connectionFactory,
  IConfiguration configuration) : base(mediator, connectionFactory, configuration)
  {
    try
    {
      var consumer = new AsyncEventingBasicConsumer(channel);
      consumer.Received += OnEventReceived;

      channel.BasicConsume(queue: "NAP.OWP.Notification.message", autoAck: false, consumer: consumer);
    }
    catch (Exception ex)
    {
      Console.WriteLine("Error while consuming message");
    }
  }

  public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

  public virtual Task StopAsync(CancellationToken cancellationToken)
  {
    Dispose();
    return Task.CompletedTask;
  }
}