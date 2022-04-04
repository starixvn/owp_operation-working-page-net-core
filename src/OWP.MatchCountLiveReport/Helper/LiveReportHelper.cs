using OWP.MatchCountLiveReport.Handlers;

namespace OWP.MatchCountLiveReport.Helper;

public static class LiveReportHelper
{
  public static string BuildFileName(QueryRequest request)
  {
    var fromDate = request.FromDate;
    var toDate = request.ToDate;
    var dateTime= fromDate.ToString("MMddyyyy");
    if (DateTime.Compare(fromDate, toDate) != 0)
    {
      dateTime=$"{dateTime} - {toDate.ToString("MMddyyyy")}";
    }
    return $"Match Count Live Report {dateTime}.xlsx";;
  }
}