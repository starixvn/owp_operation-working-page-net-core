using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using OWP.Services;
using Fanex.Caching;

namespace OWP.RabbitMQ;

public class NotificationConsumer : ConsumerBase, IHostedService
{
  private readonly IConfiguration configuration;

  private readonly LogServiceWrapper logServiceWrapper;

  public NotificationConsumer(
    IMediator mediator,
    IConfiguration configuration,
    LogServiceWrapper logServiceWrapper,
    ICache cacheManipulator
    ) : base(mediator, configuration, logServiceWrapper,cacheManipulator)
  {
    this.configuration = configuration;
    this.logServiceWrapper = logServiceWrapper;
  }

  public virtual async Task StartAsync(CancellationToken cancellationToken)
  {
  #if !DEBUG
    base.ConnectRabbitMQ();
  #endif

    var queue = configuration.GetSection("RabbitMQ:Queue").Value;

    try
    {
      var consumer = new AsyncEventingBasicConsumer(channel);
      consumer.Received += OnEventReceived;

      channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
    }
    catch (Exception ex)
    {
      await logServiceWrapper.Log("Error while consuming message", ex);
    }

    return;
  }

  public virtual Task StopAsync(CancellationToken cancellationToken)
  {
    Dispose();
    return Task.CompletedTask;
  }
}