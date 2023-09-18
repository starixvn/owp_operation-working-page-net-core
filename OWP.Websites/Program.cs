using Fanex.Data.SqlServer;
using Fanex.NAP.Client;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Signin.AspNetCore.Attribute;
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

// Add services to the container.
builder.Services.AddCorsPolicy();
builder.Services.AddControllerAuthorization();
builder.Services.AddCookieAuthentication(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddRedisSession(builder.Configuration);
builder.Services.Configure<RabbitMQSetting>(builder.Configuration.GetSection(RabbitMQSetting.RabbitMQ));
builder.Services.Configure<AccessCounterModel>(builder.Configuration.GetSection("AccessCounter"));

builder.Services.AddSignalR(builder.Configuration);
builder.Services.AddSimpleDataApiModule();
builder.Services.AddFanexSql(builder.Configuration);
builder.Services.AddNapServices(builder.Configuration);
builder.Services.AddOnebookUMServices(builder.Configuration);
builder.Services.AddFanexServices(builder.Configuration);
builder.Services.AddCoreServices();
builder.Services.AddMediator();
builder.Services.AddAutoMapper();
builder.Services.AddInternalServices();
builder.Services.AddRabbitMQ(builder.Configuration);
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new ServiceFilterAttribute(typeof(CheckUMFilterAttribute)));
    options.Filters.Add(new SessionAuthorizeAttribute());
}).AddNewtonsoftJson();

// Configure app
var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseStaticFiles();
app.UseCookiePolicy();
app.UseSession();
app.UseMiddleware<ErrorHandler>();
app.UseRouting();
app.UseCors("AllowOrigin");
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


