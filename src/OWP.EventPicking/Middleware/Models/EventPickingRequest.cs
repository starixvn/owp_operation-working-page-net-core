namespace OWP.EventPicking.Middleware.Models;

public class EventPickingRequest
{
    public DateTime Date { get; set; }

    public List<int> League { get; set; }

    public List<int> SelectGame { get; set; }

    public List<string> FilterMatch { get; set; }

    public List<string> HasProvider { get; set; }

    public List<string> HasOtherProvider { get; set; }

    public List<int> FilterMatchCategory { get; set; }

    public List<int?> UpdateType { get; set; }

    public Period PeriodOneMonth { get; set; }
    public Period PeriodThreeMonths { get; set; }
    public Period PeriodSixMonths { get; set; }
    public Period PeriodTwelveMonths { get; set; }
}

public class Period
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }
}