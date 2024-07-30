namespace OWP.DailyWorkingShift.Services
{
  

    public interface ModelService
    {
        DailyWorkingShiftPermission BuildPermissionViewModel();
    }

    public class ModelServiceImp : ModelService
    {
        private readonly PermissionService permissionService;

        public ModelServiceImp(PermissionService permissionService)
        {
            this.permissionService = permissionService;
        }

        public DailyWorkingShiftPermission BuildPermissionViewModel()
        {
            var dailyWorkingShiftFunctionPolicies = permissionService.GetPermissions(FunctionKeys.DailyWorkingShift);
            var weeklyWorkingShiftManagementPermission = permissionService.GetPermissionModel(FunctionKeys.WorkingShiftManagement);

            return new DailyWorkingShiftPermission
            {
                FullTimeView = permissionService.GetPermissionModel(FunctionKeys.FullTimeView),
                DailyWorkingShift = permissionService.GetPermissionModel(FunctionKeys.DailyWorkingShift),
                DailyWorkingShiftAllSections = dailyWorkingShiftFunctionPolicies.GetPermissionModel(FunctionKeys.DailyWorkingShift_AllSections),
                DailyWorkingShiftOwnSection = dailyWorkingShiftFunctionPolicies.GetPermissionModel(FunctionKeys.DailyWorkingShift_OwnSection),
                WeeklyWorkingShiftManagement = weeklyWorkingShiftManagementPermission
            };
        }
    }
}