using Fanex.Data;
using Newtonsoft.Json;

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
services.Configure<AccessCounterModel>(configuration.GetSection("AccessCounter"));

services.AddSignalR(configuration);
services.AddSimpleDataApiModule();
services.AddFanexData(configuration);
services.AddNapServices(configuration);
services.AddOneBookUm(configuration);
services.AddFanexServices(configuration);
services.AddGetNotificationQueue();
services.AddCoreServices();
services.AddMediator();
services.AddAutoMapper();
services.AddInternalServices();
services.AddRabbitMQ();

services.AddCarter();

services.AddWebOptimizer();
var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseWebOptimizer();
app.UseTicketPathStaticFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseCookiePolicy();
app.UseSession();
app.UseMiddleware<ErrorHandler>();

app.UseAuthentication();
app.UseAuthorization();
app.UseRabbitListener();

NapClientManager
  .UseConfig(app.Services.GetService<INapSettings>())
  .UseSession(app.Services.GetService<INapSession>());
app.MapCarter();
app.MapControllerRoute(
        "default",
        "{controller=Home}/{action=Index}/{id?}");
app.MapHub<NewDataUploadNotifyHubImp>("/otherSportNewDataUploadNotifyHub");
app.MapHub<ChangedInfoNotifyHubImp>("/changedInfoNotifyHub");
app.MapHub<TicketCenterNotifyHubImp>("/ticketCenterNotifyHub");
app.MapHub<PreMatchNotifyHubImp>("/preMatchNotifyHub");

ThreadPool.SetMinThreads(100, 100);
app.Run();


