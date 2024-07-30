namespace OWP.WorkingShift.DailyWorkingShift.Models;

using OWP.Models;

public class DailyWorkingShiftPermission
{
    public PermissionModel FullTimeView { get; set; } = new();

    public PermissionModel DailyWorkingShift { get; set; } = new();

    public PermissionModel DailyWorkingShiftAllSections { get; set; } = new();

    public PermissionModel DailyWorkingShiftOwnSection { get; set; } = new();

    public PermissionModel WeeklyWorkingShiftManagement { get; set; } = new();
}