using OWP.Event.LiveGameAssignment.Models;

namespace OWP.Event.LiveGameAssignment.Download.Models.EventAssignment;

public class DownLoadExportModel
{
    public DateTime EventDate { get; set; }

    public DateTime UploadDateTime { get; set; }

    public string Username { get; set; }

    public string MatchStatusIds { get; set; }

    public string MatchType { get; set; }

    public string OffVRs { get; set; }

    public int SyncLiveStatus { get; set; }

    public EventAssignmentPermissionModel PermissionModel { get; set; }
}
