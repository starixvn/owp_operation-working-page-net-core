using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace OWP.Websites
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var webHostBuilder = Host.CreateDefaultBuilder(args)
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      webBuilder.UseUrls($"http://*:5000;https://*:5001");
                      webBuilder.ConfigureAppConfiguration((builderContext, config) =>
                      {
                          var env = builderContext.HostingEnvironment;

                          config.SetBasePath(env.ContentRootPath);
                          config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                          config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true,
                              reloadOnChange: true);

                          config.AddEnvironmentVariables();
                      });
                      webBuilder.UseStartup<Startup>();
                  });

            return webHostBuilder;
        }
    }
}