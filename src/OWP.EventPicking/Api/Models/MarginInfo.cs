namespace OWP.EventPicking.Api.Models;

public class MarginInfo(string title)
{
    public string Title { get; set; } = title;
    public string ValueDisplay { get; set; }

    public double? Value { get; set; }

    public string Color { get; set; }
}