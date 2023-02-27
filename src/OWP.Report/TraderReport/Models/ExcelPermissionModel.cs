using OWP.Models;

namespace OWP.Report.TraderReport.Models;

public class ExcelPermissionModel
{
    public PermissionModel TraderAssignment { get; set; } = new();

    public PermissionModel TraderPerformance { get; set; } = new();
}