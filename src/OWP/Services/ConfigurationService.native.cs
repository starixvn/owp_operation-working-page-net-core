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

        public ConfigurationServiceImp(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string CurrentDirectory => configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
    }
}
