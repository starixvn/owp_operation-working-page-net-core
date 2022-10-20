namespace OWP.Report.TraderReport.Services
{
    using Constant;
    using Models;
    using OfficeOpenXml;

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
            var performaceHasRead = permission.TraderPerformace.IsRead;
            if (assignmentHasRead && performaceHasRead)
            {
                return addressColumnRemoved;
            }
            if (assignmentHasRead)
            {
                addressColumnRemoved.ColumnFrom = ReportConst.ColumnFromRemovedAssignment;
                addressColumnRemoved.Columns = ReportConst.ColumnRemovedAssignment;
            }
            if (performaceHasRead)
            {
                addressColumnRemoved.ColumnFrom = ReportConst.ColumnFromRemovedPerformace;
                addressColumnRemoved.Columns = ReportConst.ColumnRemovedPerformace;
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
}