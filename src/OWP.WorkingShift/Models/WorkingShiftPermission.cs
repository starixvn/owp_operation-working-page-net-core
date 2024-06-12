namespace OWP.WorkingShift.Models
{
    using OWP.Models;

    public class WorkingShiftPermission
    {
        public PermissionModel WorkingShiftManagement { get; set; } = new PermissionModel();

        public PermissionModel WorkingShiftManagementOwnSection { get; set; } = new PermissionModel();

        public PermissionModel WorkingShiftManagementAllSections { get; set; } = new PermissionModel();

        public PermissionModel WorkingShiftManagementShiftManagement { get; set; } = new PermissionModel();

        public PermissionModel DailyWorkingShiftManagement { get; set; } = new PermissionModel();
    }
}