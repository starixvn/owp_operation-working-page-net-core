namespace OWP.Report.TraderReport.Models
{
    using System.Collections.Generic;
    using Report.Models;

    public class ExcelModel
    {
        public IList<ExcelReportModel> ExcelModels { get; set; }

        public ExcelFileInfo FileInfo { get; set; }
    }
}