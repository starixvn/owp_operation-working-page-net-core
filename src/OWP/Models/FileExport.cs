namespace OWP.Models;

public class FileExport
{
    public string Name { get; set; }
    public string ContentType { get; set; }
    public byte[] Content { get; set; }

    public FileExport(string name, byte[] content)
    {
        Name = name;
        Content = content;
        ContentType = "application/xlsx";
    }
}