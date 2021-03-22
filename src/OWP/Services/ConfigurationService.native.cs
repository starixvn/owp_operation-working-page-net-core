namespace OWP.Services
{
    using System;

    public interface ConfigurationService
    {
        string CurrentDirectory { get; }
    }

    public class ConfigurationServiceImp : ConfigurationService
    {
        public string CurrentDirectory => AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
    }
}
