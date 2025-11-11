namespace OWP.EventPicking.Api.Middleware.Models;

public class EventPickingRequest
{
    public DateTime Date { get; set; }

    public List<int> SelectGame { get; set; }

    public List<string> FilterMatch { get; set; }

    public List<string> HasProvider { get; set; }

    public List<string> HasOtherProvider { get; set; }

    public List<int> FilterMatchCategory { get; set; }

    public List<string> UpdateType { get; set; }

    public Period Period { get; set; }

    public bool IsLoadTurnOver { get; set; } = false;
}

public class Period
{
    public TimeRange OneMonth { get; set; }
    public TimeRange ThreeMonths { get; set; }
    public TimeRange SixMonths { get; set; }
    public TimeRange TwelveMonths { get; set; }
}

public class TimeRange
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }
}