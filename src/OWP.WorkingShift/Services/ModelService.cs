namespace OWP.WorkingShift.Services
{

    public interface ModelService
    {
        WorkingShiftPermission BuildPermissionViewModel();

        int GetOrderUserId();

        int GetOrderUserId(WorkingShiftPermission permissionModel);
    }

    public class ModelServiceImp : ModelService
    {
        private readonly PermissionService permissionService;
        private readonly INapSession currentSession;

        public ModelServiceImp(PermissionService permissionService, INapSession currentSession)
        {
            this.permissionService = permissionService;
            this.currentSession = currentSession;
        }

        public WorkingShiftPermission BuildPermissionViewModel()
        {
            var workingShiftFunctionPolicies = permissionService.GetPermissions(FunctionKeys.WorkingShiftManagement);
            var dailyWorkingShiftFunctionPolicies = permissionService.GetPermissionModel(FunctionKeys.DailyWorkingShift);

            return new WorkingShiftPermission
            {
                WorkingShiftManagement = workingShiftFunctionPolicies.GetPermissionModel(FunctionKeys.WorkingShiftManagement_OwnSection),
                WorkingShiftManagementOwnSection = workingShiftFunctionPolicies.GetPermissionModel(FunctionKeys.WorkingShiftManagement_OwnSection),
                WorkingShiftManagementAllSections = workingShiftFunctionPolicies.GetPermissionModel(FunctionKeys.WorkingShiftManagement_AllSections),
                WorkingShiftManagementShiftManagement = workingShiftFunctionPolicies.GetPermissionModel(FunctionKeys.WorkingShiftManagement_ShiftManagement),
                DailyWorkingShiftManagement = dailyWorkingShiftFunctionPolicies
            };
        }

        public int GetOrderUserId(WorkingShiftPermission permissionModel)
        {
            return permissionModel.WorkingShiftManagementAllSections.IsRead ? 0 : currentSession.UserId;
        }

        public int GetOrderUserId()
        {
            var permissionModel = BuildPermissionViewModel();
            return GetOrderUserId(permissionModel);
        }
    }
}