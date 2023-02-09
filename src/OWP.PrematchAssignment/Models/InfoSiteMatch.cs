using OWP.PrematchAssignment.Models;

namespace OWP.PreMatchAssignment.Models;

public class InfoSiteMatch
{
    public string MatchOffer { get; set; } = String.Empty;

    public int InfoSiteMatchId { get; set; }

    public int AdminSiteMatchId { get; set; }

    public string LeagueName { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public MaxbetSpreadBase MaxBetHdp { get; set; } = new MaxbetSpreadBase();

    public MaxbetSpreadBase MaxBetOu { get; set; } = new MaxbetSpreadBase();

    public DateTime EventDateTime { get; set; }

    public int PreMatchLeagueId { get; set; }

    public bool HasNotes { get; set; }

    public int? PreMatchNoteId { get; set; }

    public bool IsLive { get; set; }

    public string MatchStatus { get; set; } = string.Empty;

    public List<TicketMatchBasicInfo> TicketList { get; set; } = new List<TicketMatchBasicInfo>();

    public bool HasMessages => TicketList.Count > 0;

    public bool IsRead => TicketList.All(x => x.IsRead);
}