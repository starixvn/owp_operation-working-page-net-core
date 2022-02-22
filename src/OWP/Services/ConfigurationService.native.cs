namespace OWP.Services
{
    using Microsoft.AspNetCore.Hosting;

    public interface ConfigurationService
    {
        string CurrentDirectory { get; }
    }

    public class ConfigurationServiceImp : ConfigurationService
    {
        private readonly IWebHostEnvironment hostEnvironment;

        public ConfigurationServiceImp(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public string CurrentDirectory => hostEnvironment.ContentRootPath;
    }
}