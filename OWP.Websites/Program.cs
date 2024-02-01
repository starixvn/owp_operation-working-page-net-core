using Fanex.Data.SqlServer;
using Fanex.NAP.Client;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.SignIn.Attribute;
using Microsoft.AspNetCore.Mvc;
using ONEbook.UM.AspNetCore.Filters;
using OWP.Hubs;
using OWP.Models;
using OWP.OtherSport.Hubs;
using OWP.RabbitMQ.Model;
using OWP.TicketCenter.Hubs;
using OWP.Websites;
using OWP.Websites.CrossCuttingConcern;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory()
});
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.


services.AddControllerAuthorization();
services.AddCookieAuthentication(configuration);
services.AddRedis(configuration);
services.AddRedisSession(configuration);
services.Configure<RabbitMQSetting>(configuration.GetSection(RabbitMQSetting.RabbitMQ));
services.Configure<AppSettings>(configuration.GetSection(AppSettings.Key));
services.Configure<AccessCounterModel>(configuration.GetSection("AccessCounter"));

services.AddSignalR(configuration);
services.AddSimpleDataApiModule();
services.AddFanexSql(configuration);
services.AddNapServices(configuration);
services.AddOneBookUm(configuration);
services.AddFanexServices(configuration);
services.AddCoreServices();
services.AddMediator();
services.AddAutoMapper();
services.AddInternalServices();
services.AddRabbitMQ();
services.AddCorsPolicy();

services.AddControllersWithViews(options =>
{
    options.Filters.Add(new ServiceFilterAttribute(typeof(CheckUMFilterAttribute)));
    options.Filters.Add(new SessionAuthorizeAttribute());
}).AddNewtonsoftJson();

// Configure app
var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseTicketPathStaticFiles();
app.UseRouting();

app.UseCors("CORSPolicy");

app.UseCookiePolicy();
app.UseSession();
app.UseMiddleware<ErrorHandler>();

app.UseAuthentication();
app.UseAuthorization();
app.UseRabbitListener();

NapClientManager
  .UseConfig(app.Services.GetService<INapSettings>())
  .UseSession(app.Services.GetService<INapSession>());

app.MapControllerRoute(
        "default",
        "{controller=Home}/{action=Index}/{id?}");
app.MapHub<NewDataUploadNotifyHubImp>("/otherSportNewDataUploadNotifyHub");
app.MapHub<ChangedInfoNotifyHubImp>("/changedInfoNotifyHub");
app.MapHub<TicketCenterNotifyHubImp>("/ticketCenterNotifyHub");
app.MapHub<PreMatchNotifyHubImp>("/preMatchNotifyHub");

ThreadPool.SetMinThreads(100, 100);

app.Run();


