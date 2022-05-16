using MediatR;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client.Events;
using System.Text;
using OWP.RabbitMQ.Constants;
using OWP.Services;
using Fanex.Caching;

namespace OWP.RabbitMQ;

public class ConsumerBase : RabbitMqClient
{
  private readonly IMediator mediator;

  private readonly LogServiceWrapper logServiceWrapper;

  private readonly ICache cacheManipulator;

  public ConsumerBase(
      IMediator mediator,
      IConfiguration configuration,
      LogServiceWrapper logServiceWrapper,
      ICache cacheManipulator
      ) : base(configuration,cacheManipulator)
  {
    this.mediator = mediator;
    this.logServiceWrapper = logServiceWrapper;
    this.cacheManipulator = cacheManipulator;
  }

  protected virtual async Task OnEventReceived(object sender, BasicDeliverEventArgs eventArgs)
  {
    try
    {
      var body = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
      var routingKey = eventArgs.RoutingKey;
      var message = RoutingKey.KeyMappingDict[routingKey](body);
      if (message != null)
      {
        await mediator.Send(message);
      }
    }
    catch (Exception ex)
    {
      await logServiceWrapper.Log("Error while retrieving message from queue", ex);
    }
    finally
    {
      channel.BasicAck(eventArgs.DeliveryTag, false);
    }
  }
}