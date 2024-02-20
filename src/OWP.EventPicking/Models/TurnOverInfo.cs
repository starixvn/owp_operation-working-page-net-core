namespace OWP.EventPicking.Models;

public class TurnOverInfo
{
    public TurnOverInfo(string title)
    {
        Title = title;
    }
    public string Title { get; set; }
    public string ValueDisplay { get; set; }

    public double? Value { get; set; }

    public string Color { get; set; }
}