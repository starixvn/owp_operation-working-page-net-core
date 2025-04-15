
namespace OWP.Application.Header;
public class HeaderViewModel
{
    public string Title { get; set; }

    public Header Header { get; set; }

    public bool HasDetailInformationPermission { get; set; }

    public bool HasQuickSearchPermission { get; set; }

    public bool HasRocketChatPermission { get; set; }
}
