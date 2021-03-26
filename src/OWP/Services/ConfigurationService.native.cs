namespace OWP.Services
{
    using System.IO;

    public interface ConfigurationService
    {
        string CurrentDirectory { get; }
    }

    public class ConfigurationServiceImp : ConfigurationService
    {
        public string CurrentDirectory => Directory.GetCurrentDirectory();
    }
}
