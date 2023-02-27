using OWP.Report.Models;

namespace OWP.Report.LAReport.Models;

public class LAExcelModel
{
    public IList<LAExcelReportModel> ExcelModels { get; set; }

    public ExcelFileInfo FileInfo { get; set; }
}