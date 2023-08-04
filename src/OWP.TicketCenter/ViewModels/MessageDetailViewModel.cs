using System.Runtime.CompilerServices;
using OWP.Helpers;

namespace OWP.TicketCenter.ViewModels;

public class MessageDetailViewModel
{
    public int MessageId { get; set; }

    public string Message { get; set; }

    public int UserId { get; set; }

    public string Username { get; set; }

    public int? FileId { get; set; }

    public bool IsSaved { get; set; }

    public bool IsFile => FileId.HasValue;

    public string  ImageFileName { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string QuoteMessage { get; set; }

    public string QuoteUsername { get; set; }

    public DateTime QuoteCreatedDate { get; set; }

    public byte[] ImageContent => GetImageContent();

    private byte[] GetImageContent()
    {
        if (string.IsNullOrEmpty(ImageFileName))
        {
            return null;
        }
        var filePath = FileHelper.GetFullPath(ImageFileName);
        return File.Exists(FileHelper.GetFullPath(filePath)) ? File.ReadAllBytes( filePath) : null;
    }
}