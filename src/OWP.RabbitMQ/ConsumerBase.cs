using RabbitMQ.Client;
using MediatR;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client.Events;
using System.Text;
using OWP.RabbitMQ.Constants;

namespace OWP.RabbitMQ;

public class ConsumerBase : RabbitMqClient
{
  private readonly IMediator mediator;

  public ConsumerBase(
      IMediator mediator,
      ConnectionFactory connectionFactory,
      IConfiguration configuration) :
      base(connectionFactory, configuration)
  {
    this.mediator = mediator;
  }

  protected virtual async Task OnEventReceived(object sender, BasicDeliverEventArgs eventArgs)
  {
    try
    {
      var body = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
      var routingKey = eventArgs.RoutingKey;
      var message = RoutingKey.KeyMappingDict[routingKey](body);
      Console.WriteLine($"{routingKey}");
      // await mediator.Send(message);
    }
    catch (Exception ex)
    {
      Console.WriteLine("Error while retrieving message from queue.");
    }
    finally
    {
      channel.BasicAck(eventArgs.DeliveryTag, false);
    }
  }
}