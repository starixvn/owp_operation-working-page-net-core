namespace OWP.WorkingShift.Common.Models;

using OWP.Models;

public class WorkingShiftPermission
{
    public PermissionModel WorkingShiftManagement { get; set; } = new();

    public PermissionModel WorkingShiftManagementOwnSection { get; set; } = new();

    public PermissionModel WorkingShiftManagementAllSections { get; set; } = new();

    public PermissionModel WorkingShiftManagementShiftManagement { get; set; } = new();

    public PermissionModel DailyWorkingShiftManagement { get; set; } = new();
}