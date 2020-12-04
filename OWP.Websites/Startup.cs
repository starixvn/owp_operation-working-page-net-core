using Fanex.NAP.Client;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Client.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OWP.Websites.CrossCuttingConcern;

namespace OWP.Websites
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureCORS(services);
            ConfigureAuthentication(services);
            ConfigureAuthorization(services);
            ConfigureRedis(services);
            ConfigureSession(services);

            ConfigureNapServices(services);
            ConfigureFanex();
            ConfigureCore(services);
            ConfigureMediator(services);

            ConfigInternalService(services);

            ConfigureSwagger(services);

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();
            app.UseRouting();
            app.UseCors();
            app.UseMiddleware<ErrorHandler>();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OWPNetCoreApi"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            NapClientManager
              .UseConfig(app.ApplicationServices.GetService<INapSettings>())
              .UseRestClient(app.ApplicationServices.GetService<IRestClient>())
              .UseSession(app.ApplicationServices.GetService<INapSession>());
        }
    }
}