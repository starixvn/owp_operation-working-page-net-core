namespace OWP.Helpers;
public class FileHelper
{
    public static string GetFullPath(string fileName)
    {
        var folderPath = @$"App_Data{Path.DirectorySeparatorChar}Tickets";
        var absoluteFolderPath = $"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}{folderPath}";

        if (!Directory.Exists(absoluteFolderPath))
        {
            Directory.CreateDirectory(absoluteFolderPath);
        }

        return Path.Combine(absoluteFolderPath, fileName);
    }
}
