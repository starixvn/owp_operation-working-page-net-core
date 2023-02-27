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
        var addressColumnRemoved = new AddressColumnRemoved();
        var assignmentHasRead = permission.TraderAssignment.IsRead;
        var performanceHasRead = permission.TraderPerformance.IsRead;
        if (assignmentHasRead && performanceHasRead)
        {
            return addressColumnRemoved;
        }
        if (assignmentHasRead)
        {
            addressColumnRemoved.ColumnFrom = ReportConst.ColumnFromRemovedAssignment;
            addressColumnRemoved.Columns = ReportConst.ColumnRemovedAssignment;
        }
        if (performanceHasRead)
        {
            addressColumnRemoved.ColumnFrom = ReportConst.ColumnFromRemovedPerformance;
            addressColumnRemoved.Columns = ReportConst.ColumnRemovedPerformance;
        }

        return addressColumnRemoved;
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