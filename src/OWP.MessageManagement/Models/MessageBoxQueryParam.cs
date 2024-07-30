
namespace OWP.MessageManagement.Models;
public class MessageBoxQueryParam
{
    private const int DefaultPageSize = 25;

    public string Keyword { get; set; }

    public int PageSize { get; set; } = DefaultPageSize;

    public int PageNumber { get; set; } = 1;

    public bool? IsWatcher { get; set; }
}
