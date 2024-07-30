
namespace OWP.OddsCompiler.Models;
public class MarketNotificationModel
{
    public int OWPMatchId { get; set; }

    public string OldValue { get; set; }

    public string NewValue { get; set; }

    public string AssigneeIds { get; set; }

    public string WatcherIds { get; set; }
}