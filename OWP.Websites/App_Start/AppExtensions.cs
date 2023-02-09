namespace OWP.Websites.App_Start;

using RabbitMQ;

public static class AppExtensions
{
    public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
    {

        var rabbitService = app.ApplicationServices.GetService<MessageClient>();
        var lifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();
        lifetime!.ApplicationStarted.Register(() => rabbitService!.Connect());

        return app;
    }
}