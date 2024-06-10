namespace OWP.Application.Header;

public class HeaderViewModel
{
    public string Title { get; set; }

    public HeaderModel Header { get; set; }

    public bool HasDetailInformationPermission { get; set; }

    public bool HasQuickSearchPermission { get; set; }
}
