using Fanex.Data;
using Fanex.Logging;
using Newtonsoft.Json;
using ONEbook.UM.DependencyInjection;
using OWP.Services;
using OWP.Websites;
using OWP.Websites.Controllers;
using ILogger = Fanex.Logging.ILogger;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory()
});
var services = builder.Services;
var configuration = builder.Configuration;

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
};

services.AddControllerAuthorization();
services.AddCookieAuthentication(configuration);
services.AddRedis(configuration);
services.AddRedisSession(configuration);
services.Configure<RabbitMQSetting>(configuration.GetSection(RabbitMQSetting.RabbitMQ));
services.Configure<AppSettings>(configuration.GetSection(AppSettings.Key));
services.AddSingleton<MaintenanceService>();
services.AddTransient<ILogger, Logger>();

services.AddSignalR(configuration);
services.AddSimpleDataApiModule();
services.AddFanexData(configuration);
services.AddOneBookUmForWeb(configuration);
services.AddNapClient(configuration);
services.AddFanexLogServices(configuration);
services.AddGetNotificationQueue();
services.AddCoreServices();
services.AddMediator();
services.AddAutoMapper();
services.AddInternalServices();
services.AddRabbitMQ();
services.AddCarter();
services.AddSectionHttpClientOWPChat(configuration);
services.AddWebOptimizer();
services.AddOWPLog();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseWebOptimizer();
app.UseTicketPathStaticFiles();
app.UseStaticFiles();
app.UseMiddleware<MaintenanceMiddleware>();
app.UseRouting();

app.UseCookiePolicy();
app.UseSession();
app.UseMiddleware<ChangePasswordMiddleWare>();
app.UseMiddleware<ErrorHandler>();

app.UseAuthentication();
app.UseAuthorization();
app.UseRabbitListener();

app.MapCarter();
app.MapControllerRoute(
        "default",
        "{controller=Home}/{action=Index}/{id?}");
app.MapHub<NewDataUploadNotifyHubImp>("/otherSportNewDataUploadNotifyHub");
app.MapHub<ChangedInfoNotifyHubImp>("/changedInfoNotifyHub");
app.MapHub<TicketCenterNotifyHubImp>("/ticketCenterNotifyHub");
app.MapHub<PreMatchNotifyHubImp>("/preMatchNotifyHub");

ThreadPool.SetMinThreads(100, 100);
await app.RunAsync();