using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OWP.TicketCenter.Models;

public class PrematchTicketInfo
{
    public int TicketId { get; set; }

    public string LatestUsername { get; set; }

    public DateTime LatestCreatedDate { get; set; }

    public string LatestMessage { get; set; }

    public bool IsRead { get; set; }

    public int SubtypeId { get; set; }

    public string SubtypeName { get; set; }
}

public class TicketLeagueBasicInfo : PrematchTicketInfo
{
    [JsonProperty("OWPLeagueId")]
    public int OWPLeagueId { get; set; }

    public int EventType => (int)Enum.EventType.League;
}

public class TicketMatchBasicInfo : PrematchTicketInfo
{
    public int InfoSiteMatchId { get; set; }

    public int EventType => (int)Enum.EventType.Match;
}