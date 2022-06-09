namespace OWP.OtherSport.Validation.Models;

public class OSMatchInfo
{
    public int? Id { get; set; }
    public DateTime DateTime { get; set; }
    public string League { get; set; }
    public string Home { get; set; }
    public string Away { get; set; }
    public string Sport { get; set; }
    public bool canDelete { get; set; }

    public override string ToString() => $"({DateTime.ToString("yyyy-MM-dddd H:mm:ss")},{League}, {Home}, {Away}, {Sport})";
}