namespace OWP.Models;

public class AppSettings
{
    public const string Key = "InfoSiteSettings";

    public string InfoSiteAPI { get; set; }

    public string AdminSiteAPI { get; set; }

    public bool ShowTestMatchEventPicking { get; set; }
}