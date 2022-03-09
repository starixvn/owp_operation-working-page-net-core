// using Microsoft.Extensions.Hosting;

// namespace OWP.RabbitMQ;

// public class NotificationConsumer : IHostedService
// {
//   private readonly RabbitMqClient rabbitMqClient;

//   public NotificationConsumer(RabbitMqClient rabbitMqClient)
//   {
//     this.rabbitMqClient = rabbitMqClient;
//   }

//   public Task StartAsync(CancellationToken cancellationToken)
//   {
//     rabbitMqClient.OnEventReceived();

//     return Task.CompletedTask;
//   }

//   public Task StopAsync(CancellationToken cancellationToken)
//   {
//     return Task.CompletedTask;
//   }
// }