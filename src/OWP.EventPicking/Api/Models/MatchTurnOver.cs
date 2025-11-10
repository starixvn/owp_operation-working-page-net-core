using Newtonsoft.Json;

namespace OWP.EventPicking.Api.Models;

public class MatchTurnOver 
{
    public int InfositeMatchId { get; set; }

    public string LeagueName { get; set; }

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

}

