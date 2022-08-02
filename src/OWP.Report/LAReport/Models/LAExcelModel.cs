namespace OWP.Report.LAReport.Models
{
    using System.Collections.Generic;
    using Report.Models;

    public class LAExcelModel
    {
        public IList<LAExcelReportModel> ExcelModels { get; set; }

        public ExcelFileInfo FileInfo { get; set; }
    }
}