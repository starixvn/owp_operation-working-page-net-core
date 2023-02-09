using System.Diagnostics;
using System.Text;
using Fanex.NAP.Client;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Signin.AspNetCore.Attribute;
using Microsoft.AspNetCore.Mvc;
using ONEbook.UM.AspNetCore.Filters;
using OWP.Hubs;
using OWP.OtherSport.Hubs;
using OWP.RabbitMQ.Model;
using OWP.TicketCenter.Hubs;
using OWP.Websites;
using OWP.Websites.App_Start;
using OWP.Websites.CrossCuttingConcern;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
});

// Add services to the container.
builder.Services.AddCorsPolicy();
builder.Services.AddControllerAuthorization();
builder.Services.AddCookieAuthentication(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddRedisSession(builder.Configuration);
builder.Services.Configure<RabbitMQSetting>(builder.Configuration.GetSection(RabbitMQSetting.RabbitMQ));

builder.Services.AddSignalR(builder.Configuration);
builder.Services.AddSimpleDataApiModule();
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
void WriteLogWhenApplicationIsStopping(object title)
{
    Console.WriteLine(title);

    using var currentProcess = Process.GetCurrentProcess();
    var stringBuilder = new StringBuilder();

    stringBuilder.AppendFormat("GC.GetTotalMemory(false): {0:n0}\n", GC.GetTotalMemory(false));
    stringBuilder.AppendFormat("VirtualMemorySize64 is: {0:n0}\n", currentProcess.VirtualMemorySize64);
    stringBuilder.AppendFormat("PrivateMemorySize64 is: {0:n0}\n", currentProcess.PrivateMemorySize64);
    stringBuilder.AppendFormat("WorkingSet64 is: {0:n0}\n", currentProcess.WorkingSet64);
    stringBuilder.AppendFormat("PagedMemorySize64 is: {0:n0}\n", currentProcess.PagedMemorySize64);
    stringBuilder.AppendFormat("NonpagedSystemMemorySize64 is: {0:n0}\n", currentProcess.NonpagedSystemMemorySize64);
    stringBuilder.AppendFormat("NonpagedSystemMemorySize64 is: {0:n0}\n", currentProcess.NonpagedSystemMemorySize64);
    stringBuilder.AppendLine($@"Threads.Count: {currentProcess.Threads.Count}");
    stringBuilder.AppendLine($@"ThreadPool.PendingWorkItemCount: {ThreadPool.PendingWorkItemCount}");

    Console.WriteLine(stringBuilder.ToString());
}
app.Lifetime.ApplicationStopping.Register(WriteLogWhenApplicationIsStopping, @"ApplicationStopping was called");
app.Lifetime.ApplicationStopped.Register(WriteLogWhenApplicationIsStopping, @"ApplicationStopped was called");

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
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<NewDataUploadNotifyHubImp>("/otherSportNewDataUploadNotifyHub");
app.MapHub<ChangedInfoNotifyHubImp>("/changedInfoNotifyHub");
app.MapHub<TicketCenterNotifyHubImp>("/ticketCenterNotifyHub");
app.MapHub<PreMatchNotifyHubImp>("/preMatchNotifyHub");

ThreadPool.SetMinThreads(100, 100);

#if DEBUG
app.Run();
#endif

await app.RunAsync("http://0.0.0.0:5000");

