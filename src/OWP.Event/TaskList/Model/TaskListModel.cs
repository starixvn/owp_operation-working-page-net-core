using OWP.Event.EventShared.Models;

namespace OWP.Event.TaskList.Model;
public class TaskListModel
{
    public bool HasReadNotePermission { get; set; }

    public IEnumerable<MatchInfo> Matches { get; set; }

    public bool HasDetailInformationPermission { get; set; }

    public string Today { get; set; }

    public string Yesterday { get; set; }
}
