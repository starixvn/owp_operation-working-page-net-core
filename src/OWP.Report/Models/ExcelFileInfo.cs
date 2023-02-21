using Epplus.Mapper.Annotations;

namespace OWP.Report.Models;

public class ExcelFileInfo
{
    [Cell("B1")]
    public string FromDate { get; set; }

    [Cell("B2")]
    public string ToDate { get; set; }

    [Cell("B3")]
    public string ExportBy { get; set; }

    [Cell("B4")]
    public string ExportTime { get; set; }
}