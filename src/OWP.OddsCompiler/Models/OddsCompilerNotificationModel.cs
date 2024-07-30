

namespace OWP.OddsCompiler.Models;
public class OddsCompilerNotificationModel
{
    public int OWPMatchId { get; set; }

    public int Bettype { get; set; }

    public string AssigneeIds { get; set; }

    public string WatcherIds { get; set; }
}