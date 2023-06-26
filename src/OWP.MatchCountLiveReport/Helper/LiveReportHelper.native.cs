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
      dateTime = $"{dateTime} - {toDate:ddMMMyyyy}";
    }
    return $"Match Count Live Report {dateTime}.xlsx";
  }

  public static string BuildCellValue(int? source)
  => source.HasValue
  ? $"{source.Value:n0}"
  : "-";
}