namespace OWP.Event.LiveGameAssignment.EventPicking.Model;

public class EventInfo
{
    public int OWPMatchId { get; set; }

    public int? MinorBettypeId { get; set; }

    public int? ParentMatchId { get; set; }

    public long InfositeId { get; set; }

    public string MatchNo { get; set; }

    public string MatchStatus { get; set; }

    public TimeSpan? OtherTime { get; set; }

    public string TraderHDP { get; set; }

    public string TraderOU { get; set; }

    public string Trader1HHDP { get; set; }

    public string Trader1HOU { get; set; }

    public string OtherTradersHDP { get; set; }

    public string OtherTradersOU { get; set; }

    public string OtherTraders1HHDP { get; set; }

    public string OtherTraders1HOU { get; set; }

    public string X15mins { get; set; }

    public string X10mins { get; set; }

    public string Trader1X2 { get; set; }

    public string OtherTraders1X2 { get; set; }

    public string FM { get; set; }

    public string AIO { get; set; }

    public string LA { get; set; }

    public string OtherLA { get; set; }

    public string LASystemAssign { get; set; }

    public string Remark { get; set; }

    public string ReporterPhone { get; set; }

    public string OtherTraders15mins { get; set; }

    public string OtherTraders10mins { get; set; }

    public string OtherTradersFM { get; set; }

    public string OtherTradersAIO { get; set; }

    public string Trader1H1X2 { get; set; }

    public string OtherTraders1H1X2 { get; set; }

    public string MaxBetXmins { get; set; }

    public string SpreadXmins { get; set; }

    public string MaxBetFM { get; set; }

    public string SpreadFM { get; set; }
}

public class MatchAssignment
{
    public int OWPMatchId { get; set; }

    public int? ParentMatchId { get; set; }

    public int Bettype { get; set; }

    public int? MinorBettypeId { get; set; }

    public int AssignmentType { get; set; }

    public int UserId { get; set; }

    public string Username { get; set; }

    public string AssignmentNote { get; set; }
}

public class MatchSetting
{
    public int OWPMatchId { get; set; }

    public int? ParentMatchId { get; set; }

    public int Bettype { get; set; }

    public int? MinorBettypeId { get; set; }

    public decimal? Spread { get; set; }

    public decimal? MaxBet { get; set; }
}
