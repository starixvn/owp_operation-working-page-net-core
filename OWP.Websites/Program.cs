using Fanex.NAP.Client;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Web;
using OWP.Hubs;
using OWP.OtherSport.Hubs;
using OWP.TicketCenter.Hubs;
using OWP.Websites;
using OWP.Websites.CrossCuttingConcern;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
});

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddCorsPolicy();
builder.Services.AddControllerAuthorization();
builder.Services.AddCookieAuthentication(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddRedisSession(builder.Configuration);
builder.Services.AddSignalR(builder.Configuration);
builder.Services.AddSimpleDataApiModule();
builder.Services.AddNapServices(builder.Configuration);
builder.Services.AddFanexServices(builder.Configuration);
builder.Services.AddCoreServices();
builder.Services.AddMediator();
builder.Services.AddAutoMapper();
builder.Services.AddInternalServices();
builder.Services.AddRabbitMQ(builder.Configuration);
builder.Services.AddRedisPubSub(builder.Configuration);
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

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

NapClientManager
  .UseConfig(app.Services.GetService<INapSettings>())
  .UseSession(app.Services.GetService<INapSession>());

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapHub<NewDataUploadNotifyHubImp>("/otherSportNewDataUploadNotifyHub");
    endpoints.MapHub<ChangedInfoNotifyHubImp>("/changedInfoNotifyHub");
    endpoints.MapHub<TicketCenterNotifyHubImp>("/ticketCenterNotifyHub");
    endpoints.MapHub<PreMatchNotifyHubImp>("/preMatchNotifyHub");
});

app.Urls.Add("http://+:5000");
if (app.Environment.IsDevelopment())
{
    app.Urls.Add("https://+:5001");
}
await app.RunAsync();


