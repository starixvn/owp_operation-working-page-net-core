namespace OWP.PreMatchReport.Models;

internal class ExportPreMatchModel
{
    public string TraderName { get; set; }
    public int TotalMatches { get; set; }
    public int TotalLeague { get; set; }

    // hdp league
    public int LeagueHdpMj { get; set; }

    public int LeagueHdpM1 { get; set; }
    public int LeagueHdpM2 { get; set; }
    public int LeagueHdpM3 { get; set; }
    public int LeagueHdpM4 { get; set; }
    public int LeagueHdpOthers { get; set; }
    public int LeagueHdpOthers1 { get; set; }
    public int LeagueHdpOthers2 { get; set; }
    public int LeagueHdpOthers3 { get; set; }
    public int LeagueHdpOthers4 { get; set; }
    public int LeagueHdpOthers5 { get; set; }
    public int LeagueHdpUnknown { get; set; }
    public int LeagueHdpTotal { get; set; }

    // ou league
    public int LeagueOuMj { get; set; }

    public int LeagueOuM1 { get; set; }
    public int LeagueOuM2 { get; set; }
    public int LeagueOuM3 { get; set; }
    public int LeagueOuM4 { get; set; }
    public int LeagueOuOthers { get; set; }
    public int LeagueOuOthers1 { get; set; }
    public int LeagueOuOthers2 { get; set; }
    public int LeagueOuOthers3 { get; set; }
    public int LeagueOuOthers4 { get; set; }
    public int LeagueOuOthers5 { get; set; }
    public int LeagueOuUnknown { get; set; }
    public int LeagueOuTotal { get; set; }

    //hdp match
    public int MatchHdpMj { get; set; }

    public int MatchHdpM1 { get; set; }
    public int MatchHdpM2 { get; set; }
    public int MatchHdpM3 { get; set; }
    public int MatchHdpM4 { get; set; }
    public int MatchHdpOthers { get; set; }
    public int MatchHdpOthers1 { get; set; }
    public int MatchHdpOthers2 { get; set; }
    public int MatchHdpOthers3 { get; set; }
    public int MatchHdpOthers4 { get; set; }
    public int MatchHdpOthers5 { get; set; }
    public int MatchHdpUnknown { get; set; }
    public int MatchHdpTotal { get; set; }

    // ou match\
    public int MatchOuMj { get; set; }

    public int MatchOuM1 { get; set; }
    public int MatchOuM2 { get; set; }
    public int MatchOuM3 { get; set; }
    public int MatchOuM4 { get; set; }
    public int MatchOuOthers { get; set; }
    public int MatchOuOthers1 { get; set; }
    public int MatchOuOthers2 { get; set; }
    public int MatchOuOthers3 { get; set; }
    public int MatchOuOthers4 { get; set; }
    public int MatchOuOthers5 { get; set; }
    public int MatchOuUnknown { get; set; }
    public int MatchOuTotal { get; set; }
}