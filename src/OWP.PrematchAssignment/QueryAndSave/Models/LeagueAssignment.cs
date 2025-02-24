namespace OWP.PrematchAssignment.QueryAndSave.Models;

public class LeagueAssignment
{
    public int No { get; set; }

    public int LeagueId { get; set; }

    public string LeagueGroup { get; set; }

    public int? LeagueGroupOrder { get; set; }

    public int PreMatchLeagueId { get; set; }

    public string LeagueName { get; set; }

    public int MatchCount { get; set; }

    public string HDP { get; set; }

    public string OU { get; set; }

    public string HDPTakeOver { get; set; }

    public string OUTakeOver { get; set; }

    public string OtherHDP { get; set; }

    public string OtherOU { get; set; }

    public string OtherHDPTakeOver { get; set; }

    public string OtherOUTakeOver { get; set; }

    public string HDPHint { get; set; }

    public string OUHint { get; set; }

    public string HDPTakeOverHint { get; set; }

    public string OUTakeOverHint { get; set; }

    public string OtherHDPHint { get; set; }

    public string OtherOUHint { get; set; }

    public string OtherHDPTakeOverHint { get; set; }

    public string OtherOUTakeOverHint { get; set; }

    public decimal? Spread { get; set; }

    public decimal? Maxbet { get; set; }

    public bool Blacklist { get; set; }

    public string Remark { get; set; }

    public bool HasMessages => TicketList.Count > 0;

    public bool IsRead => TicketList.TrueForAll(x => x.IsRead);

    public bool HasNotes { get; set; }

    public int? PreMatchNoteId { get; set; }

    public bool IsEarlyOffered { get; set; }

    public List<TicketLeagueBasicInfo> TicketList { get; set; } = [];

    public bool IsSaba => LeagueName.Contains("SABA", StringComparison.CurrentCultureIgnoreCase);
}