namespace OWP.EventPicking.Models;

public class LogModel
{
    public TimeSpan Date { get; set; }

    public string ModifiedBy { get; set; }

    public string Action { get; set; }

    public string OldValue { get; set; }

    public string NewValue { get; set; }
}