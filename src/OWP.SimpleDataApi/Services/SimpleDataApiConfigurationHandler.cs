namespace OWP.SimpleDataApi.Services;

public class SimpleDataApiConfigurationHandler
{
    private static SimpleDataApiConfigurationHandler instance = new();

    public static SimpleDataApiConfigurationHandler Instance()
    {
        return instance;
    }

    public SimpleDataApiConfigurationHandler SetActorIdParameterName(string name)
    {
        ParameterConfiguration.ActorId = name;
        return instance;
    }
}