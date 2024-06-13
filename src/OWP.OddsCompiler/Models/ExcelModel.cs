
using System.Collections.Generic;

namespace OWP.OddsCompiler.Models;
public class ExcelModel
{
    public IList<ExcelReportModel> ExcelModels { get; set; }

    public ExcelFileInfo FileInfo { get; set; }
}