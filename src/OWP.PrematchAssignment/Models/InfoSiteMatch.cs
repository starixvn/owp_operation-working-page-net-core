namespace OWP.PrematchAssignment.Models;

public class InfoSiteMatch
{
    public string MatchOffer { get; set; } = string.Empty;

    public int InfoSiteMatchId { get; set; }

    public int AdminSiteMatchId { get; set; }

    public int LeagueId { get; set; }

    public string LeagueName { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public string TeamRound { get; set; }

    public MaxbetSpreadBase MaxBetHdp { get; set; } = new();

    public MaxbetSpreadBase MaxBetOu { get; set; } = new();

    public DateTime EventDateTime { get; set; }

    public int PreMatchLeagueId { get; set; }

    public bool HasNotes { get; set; }

    public int? PreMatchNoteId { get; set; }

    public bool IsLive { get; set; }

    public string MatchStatus { get; set; } = string.Empty;

    public List<TicketMatchBasicInfo> TicketList { get; set; } = new();

    public bool HasMessages => TicketList.Count > 0;

    public bool IsRead => TicketList.TrueForAll(x => x.IsRead);
}