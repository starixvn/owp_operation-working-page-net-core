using OWP.Event.EventShared.Models;
namespace OWP.Event.LiveGameAssignment.EventPicking.Model;

using System.Collections.Generic;
using ViewModels;

public class EventPickingModel : Pagination
{
    public EventPickingModel()
    {
        MatchInfos = new List<MatchInfo>();
    }

    public IEnumerable<MatchInfo> MatchInfos { get; set; }

    public bool IsHost { get; set; }
}