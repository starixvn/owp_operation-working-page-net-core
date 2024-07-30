namespace OWP.Event.LiveGameAssignment.Models;

using OWP.Models;

public class EventAssignmentPermissionModel
{
    public PermissionModel LiveGameAssignment_BetTypes1X2_AIO_FM { get; set; } = new();

    public PermissionModel LiveGameAssignment_FullBetTypes { get; set; } = new();

    public PermissionModel FullTimeView { get; set; } = new();
}