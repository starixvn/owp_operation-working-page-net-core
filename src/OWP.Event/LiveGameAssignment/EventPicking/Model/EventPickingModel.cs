using OWP.Event.EventShared.Models;
namespace OWP.Event.LiveGameAssignment.EventPicking.Model;

using OWP.Event.LiveGameAssignment.EventPicking.Model.ViewModels;
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