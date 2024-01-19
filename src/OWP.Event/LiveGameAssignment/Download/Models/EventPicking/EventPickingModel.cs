using OWP.Event.EventShared.Models;
namespace OWP.Event.LiveGameAssignment.Download.Models.EventPicking;

using OWP.Event.LiveGameAssignment.Download.ViewModels.EventPicking;
using System.Collections.Generic;

public class EventPickingModel : Pagination
{
    public EventPickingModel()
    {
        MatchInfos = new List<MatchInfo>();
    }

    public IEnumerable<MatchInfo> MatchInfos { get; set; }

    public bool IsHost { get; set; }
}