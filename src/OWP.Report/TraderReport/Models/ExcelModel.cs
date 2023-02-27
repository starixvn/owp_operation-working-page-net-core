using OWP.Report.Models;

namespace OWP.Report.TraderReport.Models;

public class ExcelModel
{
    public IList<ExcelReportModel> ExcelModels { get; set; }

    public ExcelFileInfo FileInfo { get; set; }
}