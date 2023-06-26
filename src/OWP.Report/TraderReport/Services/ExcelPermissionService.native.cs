using OfficeOpenXml;
using OWP.Report.Constant;
using OWP.Report.TraderReport.Models;

namespace OWP.Report.TraderReport.Services;

public interface ExcelPermissionService
{
    void RemoveColumnRelyOnPermission(ExcelPackage excelPackage, ExcelPermissionModel permission);
}

public class ExcelPermissionServiceImp : ExcelPermissionService
{
    public void RemoveColumnRelyOnPermission(ExcelPackage excelPackage, ExcelPermissionModel permission)
    {
        var addressColumnRemoved = BuildAddressColumnRemoved(permission);
        Remove(excelPackage, addressColumnRemoved);
    }

    public static AddressColumnRemoved BuildAddressColumnRemoved(ExcelPermissionModel permission)
    {
        var assignmentHasRead = permission.TraderAssignment.IsRead;
        var performanceHasRead = permission.TraderPerformance.IsRead;

        return (assignmentHasRead, performanceHasRead) switch
        {
            (true, true) => new AddressColumnRemoved(),
            (true, false) => new AddressColumnRemoved
            {
                ColumnFrom = ReportConst.ColumnFromRemovedAssignment,
                Columns = ReportConst.ColumnRemovedAssignment
            },
            (false, true) => new AddressColumnRemoved
            {
                ColumnFrom = ReportConst.ColumnFromRemovedPerformance,
                Columns = ReportConst.ColumnRemovedPerformance
            },
            _ => new AddressColumnRemoved()
        };
    }

    public static void Remove(ExcelPackage excelPackage, AddressColumnRemoved address)
    {
        if (address.ColumnFrom <= 0 || address.Columns <= 0)
        {
            return;
        }

        var reportSheet = excelPackage.Workbook.Worksheets[ReportConst.SheetName];
        reportSheet.DeleteColumn(address.ColumnFrom, address.Columns);
    }
}