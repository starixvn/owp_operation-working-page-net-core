using OWP.Constants;
using OWP.Extensions;
using OWP.Report.TraderReport.Models;
using OWP.Services;

namespace OWP.Report.TraderReport.Services;

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
            TraderPerformance = reportFunctionPolicies.GetPermissionModel(FunctionKeys.TraderReport_TraderPerformance)
        };
    }
}