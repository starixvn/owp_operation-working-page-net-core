using OWP.Services;

namespace OWP.Event.LiveGameAssignment.Services;

using Constants;
using Extensions;
using Models;

public interface EventAssignmentPermission
{
    EventAssignmentPermissionModel BuildPermission();
}

public class EventAssignmentPermissionImp : EventAssignmentPermission
{
    private readonly PermissionService permissionService;

    public EventAssignmentPermissionImp(PermissionService permissionService)
    {
        this.permissionService = permissionService;
    }

    public EventAssignmentPermissionModel BuildPermission()
    {
        var eventAssignmentFunctionPolicies = permissionService.GetPermissions(FunctionKeys.LiveGameAssignment);

        return new EventAssignmentPermissionModel
        {
            FullTimeView = permissionService.GetPermissionModel(FunctionKeys.FullTimeView),
            LiveGameAssignment_BetTypes1X2_AIO_FM = eventAssignmentFunctionPolicies.GetPermissionModel(FunctionKeys.LiveGameAssignment_PHBetTypes),
            LiveGameAssignment_FullBetTypes = eventAssignmentFunctionPolicies.GetPermissionModel(FunctionKeys.LiveGameAssignment_FullBetTypes)
        };
    }
}
