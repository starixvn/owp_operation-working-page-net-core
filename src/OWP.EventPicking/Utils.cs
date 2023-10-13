namespace OWP.EventPicking;

public class Utils
{
    public static string GetMiscMatchTemplate(string currentDirectory)
    {
        const string templatePath = @"App_Data/Templates/EventPickingNewFormat.xlsx";
        return $"{currentDirectory}{Path.DirectorySeparatorChar}{templatePath}";
    }
}