using RabbitMQ.Client;
using MediatR;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client.Events;
using System.Text;
using OWP.RabbitMQ.Constants;
using OWP.Services;

namespace OWP.RabbitMQ;

public class ConsumerBase : RabbitMqClient
{
  private readonly IMediator mediator;

  private readonly LogServiceWrapper logServiceWrapper;

  public ConsumerBase(
      IMediator mediator,
      ConnectionFactory connectionFactory,
      IConfiguration configuration,
      LogServiceWrapper logServiceWrapper
      ) : base(connectionFactory, configuration)
  {
    this.mediator = mediator;
    this.logServiceWrapper = logServiceWrapper;
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