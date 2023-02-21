using Newtonsoft.Json;

namespace OWP.AssignmentUpload.UnitTests.TestUtility;

internal static class TestDataLoader
{
    internal static T Load<T>(string filename)
    {
        var directory = $"{Environment.CurrentDirectory}\\TestData";
        var dataFilePath = $"{directory}\\{filename}";
        var text = File.ReadAllText(dataFilePath);

        return JsonConvert.DeserializeObject<T>(text);
    }
}