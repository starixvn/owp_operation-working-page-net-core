using OWP.MatchCountLiveReport.Handlers;

namespace OWP.MatchCountLiveReport.Helper;

public static class LiveReportHelper
{
  public static string BuildFileName(QueryRequest request)
  {
    var fromDate = request.FromDate;
    var toDate = request.ToDate;
    var dateTime = fromDate.ToString("ddMMMyyyy");
    if (DateTime.Compare(fromDate, toDate) != 0)
    {
      dateTime = $"{dateTime} - {toDate.ToString("ddMMMyyyy")}";
    }
    return $"Match Count Live Report {dateTime}.xlsx";
  }

  public static string BuildCellValue(int? source)
  => source.HasValue
  ? string.Format("{0:n0}", source.Value)
  : "-";
}