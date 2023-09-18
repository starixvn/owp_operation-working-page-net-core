namespace OWP.EventPicking.Models;

public class LogModel
{
    public DateTime Date { get; set; }

    public string ModifiedBy { get; set; }

    public string Action { get; set; }

    public string OldValue { get; set; }

    public string NewValue { get; set; }
}