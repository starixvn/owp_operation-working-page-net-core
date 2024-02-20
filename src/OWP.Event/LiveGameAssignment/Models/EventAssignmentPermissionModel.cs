namespace OWP.Event.LiveGameAssignment.Models;

using OWP.Models;

public class EventAssignmentPermissionModel
{
    public PermissionModel LiveGameAssignment_BetTypes1X2_AIO_FM { get; set; } = new PermissionModel();

    public PermissionModel LiveGameAssignment_FullBetTypes { get; set; } = new PermissionModel();

    public PermissionModel FullTimeView { get; set; } = new PermissionModel();
}