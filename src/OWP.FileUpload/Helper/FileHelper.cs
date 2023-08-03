namespace OWP.FileUpload.Helper;

public static class FileHelper
{
    public static string GetMediaType(string fileTitle)
    {
        var fileExtension = fileTitle[(fileTitle.LastIndexOf('.') + 1)..].ToLowerInvariant().Trim();
        var isImage = IsImage(fileExtension);
        return isImage
            ? $"image/{fileExtension}"
            : "application/octet-stream";
    }

    private static bool IsImage(string fileExtension)
    {
        string[] imageExtension = { "jpg", "jpeg", "png", "gif", "bmp" };
        return imageExtension.Contains(fileExtension);
    }

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
