namespace OWP.Event.LiveGameAssignment.Models;

using OWP.Models;
using System;

public class EventQueryModel
{
    public DateTime EventDate { get; set; }

    public string Username { get; set; }

    public int[] MatchStatusIds { get; set; }

    public int?[] SelectedOffVRs { get; set; }

    public int SyncLiveStatus { get; set; }

    public string MatchType { get; set; }

    public PermissionModel FullTimeViewPermission { get; set; } = new PermissionModel();
}
