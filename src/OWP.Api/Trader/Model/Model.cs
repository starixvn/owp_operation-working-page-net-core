namespace OWP.Api.Trader.Model;

public class Request
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public List<MatchRequest> Matches { get; set; }
}

public class MatchRequest
{
    public int MatchId { get; set; }
    public int ParentId { get; set; }
    public int Subtype { get; set; }
}

public class OWPMatchRequest : MatchRequest
{
    public int? MinorBettypeId { get; set; }

    public bool IsMainMatch { get; set; }
}

public class OWPAssignment
{
    public int MatchId { get; set; }
    public int UserId { get; set; }
    public int Bettype { get; set; }
}

public class TraderMatch
{
    public int MatchId { get; set; }

    public List<AssignedTrader> Traders { get; set; }
}

public class AssignedTrader
{
    public int Bettype { get; set; }

    public int TraderId { get; set; }
}
