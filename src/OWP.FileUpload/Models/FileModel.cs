namespace OWP.FileUpload.Models;

public class FileModel
{
    public int FileId { get; set; }

    public string Title { get; set; }

    public string SavedFileName { get; set; }

    public string FilePath { get; set; }

    public string MediaType { get; set; }
}