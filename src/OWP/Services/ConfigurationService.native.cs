namespace OWP.Services
{
    using Microsoft.AspNetCore.Hosting;

    public interface ConfigurationService
    {
        string CurrentDirectory { get; }
    }

    public class ConfigurationServiceImp : ConfigurationService
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public ConfigurationServiceImp(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public string CurrentDirectory => hostingEnvironment.ContentRootPath;
    }
}
