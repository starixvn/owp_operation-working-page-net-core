namespace OWP.Report.TraderReport.Models;

using OWP.Models;

public class ExcelPermissionModel
{
    public PermissionModel TraderAssignment { get; set; } = new();

    public PermissionModel TraderPerformace { get; set; } = new();
}