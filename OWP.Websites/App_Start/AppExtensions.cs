using OWP.RabbitMQ;

namespace OWP.Websites;

public static class AppExtensions
{
    public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
    {
        var rabbitService = app.ApplicationServices.GetService<MessageClient>();
        var lifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();
        lifetime.ApplicationStarted.Register(() => rabbitService.Connect());

        return app;
    }
}