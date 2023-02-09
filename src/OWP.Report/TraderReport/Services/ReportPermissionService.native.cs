namespace OWP.Report.TraderReport.Services;

using Constants;
using Extensions;
using Models;
using OWP.Services;

public interface ReportPermissionService
{
    ExcelPermissionModel BuildPermission();
}

public class ReportPermissionServiceImp : ReportPermissionService
{
    private readonly PermissionService permissionService;

    public ReportPermissionServiceImp(PermissionService permissionService)
    {
        this.permissionService = permissionService;
    }

    public ExcelPermissionModel BuildPermission()
    {
        var reportFunctionPolicies = permissionService.GetPermissions(FunctionKeys.TraderReport);

        return new ExcelPermissionModel
        {
            TraderAssignment = reportFunctionPolicies.GetPermissionModel(FunctionKeys.TraderReport_TraderAssignment),
            TraderPerformace = reportFunctionPolicies.GetPermissionModel(FunctionKeys.TraderReport_TraderPerformance)
        };
    }
}