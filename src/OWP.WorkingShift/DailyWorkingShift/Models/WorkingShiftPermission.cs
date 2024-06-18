namespace OWP.WorkingShift.DailyWorkingShift.Models;

using OWP.Models;

public class DailyWorkingShiftPermission
{
    public PermissionModel FullTimeView { get; set; } = new PermissionModel();

    public PermissionModel DailyWorkingShift { get; set; } = new PermissionModel();

    public PermissionModel DailyWorkingShiftAllSections { get; set; } = new PermissionModel();

    public PermissionModel DailyWorkingShiftOwnSection { get; set; } = new PermissionModel();

    public PermissionModel WeeklyWorkingShiftManagement { get; set; } = new PermissionModel();
}