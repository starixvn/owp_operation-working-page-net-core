namespace OWP.Services
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public interface ConfigurationService
    {
        string CurrentDirectory { get; }
    }

    public class ConfigurationServiceImp : ConfigurationService
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;

        public ConfigurationServiceImp(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }

        public string CurrentDirectory => env.ContentRootPath;
    }
}
