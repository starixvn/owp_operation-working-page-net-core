namespace OWP.AssignmentUpload.Validation.Models;

public class AssignmentModel
{
    public AssignmentModel()
    {
        HashKey = string.Empty;
        SessionId = string.Empty;
        IsNeutral = false;
        IsSaba = false;
        MatchStatus = string.Empty;
        MatchNo = string.Empty;
        Home = string.Empty;
        Away = string.Empty;
        League = string.Empty;
        Agent = string.Empty;
        Channel = string.Empty;
        IPTV = string.Empty;
        Division = string.Empty;
        MatchOffer = string.Empty;
        NoHDP = string.Empty;
        Notes = [];
        OtherProvider = string.Empty;
        Phone = string.Empty;
        Provider = string.Empty;
        Remark = string.Empty;
        AgentEst = string.Empty;
        RemarkEst = string.Empty;
        RemarkBy = string.Empty;
        Round = string.Empty;
        TV = string.Empty;
        WB = string.Empty;
        Website1 = string.Empty;
        Website2 = string.Empty;
        Website3 = string.Empty;
        Website4 = string.Empty;
        OtherTraders1H1X2 = string.Empty;
        OtherTraders10Mins = string.Empty;
        OtherTraders15Mins = string.Empty;
        OtherTraders1HHDP = string.Empty;
        OtherTraders1HOU = string.Empty;
        OtherTraders1X2 = string.Empty;
        OtherTradersAIO = string.Empty;
        OtherTradersFM = string.Empty;
        OtherTradersHDP = string.Empty;
        OtherTradersOU = string.Empty;
        OtherTradersLA = string.Empty;
        ParentHashKey = string.Empty;
    }

    public int OWPMatchId { get; set; }

    public bool IsNeutral { get; set; }

    public string HashKey { get; set; }

    public string SessionId { get; set; }

    public DateTime EventDate { get; set; }

    public string MatchStatus { get; set; }

    public int InfoSiteMatchId { get; set; }

    public int AdminSiteId { get; set; }

    public string MatchNo { get; set; }

    public TimeSpan KickOffTime { get; set; }

    public TimeSpan? OtherTime { get; set; }

    public string Home { get; set; }

    public string Away { get; set; }

    public string League { get; set; }

    public bool IsSaba { get; set; }

    public string Agent { get; set; }

    public string Channel { get; set; }

    public string IPTV { get; set; }

    public string Division { get; set; }

    public string MatchOffer { get; set; }

    public string NoHDP { get; set; }

    public List<string> Notes { get; set; }

    public string OtherProvider { get; set; }

    public string Phone { get; set; }

    public string Provider { get; set; }

    public string Remark { get; set; }

    public string AgentEst { get; set; }

    public string RemarkEst { get; set; }

    public string RemarkBy { get; set; }

    public string Round { get; set; }

    public string TV { get; set; }

    public string WB { get; set; }

    public string Website1 { get; set; }

    public string Website2 { get; set; }

    public string Website3 { get; set; }

    public string Website4 { get; set; }

    public int Trader10Mins { get; set; }

    public int Trader15Mins { get; set; }

    public int Trader1H1X2 { get; set; }

    public int Trader1HHDP { get; set; }

    public int Trader1HOU { get; set; }

    public int Trader1X2 { get; set; }

    public int TraderAIO { get; set; }

    public int TraderFM { get; set; }

    public int TraderHDP { get; set; }

    public int TraderLA { get; set; }

    public int TraderOU { get; set; }

    public string OtherTraders1H1X2 { get; set; }

    public string OtherTraders10Mins { get; set; }

    public string OtherTraders15Mins { get; set; }

    public string OtherTraders1HHDP { get; set; }

    public string OtherTraders1HOU { get; set; }

    public string OtherTraders1X2 { get; set; }

    public string OtherTradersAIO { get; set; }

    public string OtherTradersFM { get; set; }

    public string OtherTradersHDP { get; set; }

    public string OtherTradersOU { get; set; }

    public string OtherTradersLA { get; set; }

    public decimal? MaxBet1H1X2 { get; set; }

    public decimal? MaxBet1HHDP { get; set; }

    public decimal? MaxBet1HOU { get; set; }

    public decimal? MaxBet1X2 { get; set; }

    public decimal? MaxBetFM { get; set; }

    public decimal? MaxBetHDP { get; set; }

    public decimal? MaxBetOU { get; set; }

    public decimal? MaxBetXMins { get; set; }

    public decimal? Spread1H1X2 { get; set; }

    public decimal? Spread1HHDP { get; set; }

    public decimal? Spread1HOU { get; set; }

    public decimal? Spread1X2 { get; set; }

    public decimal? SpreadFM { get; set; }

    public decimal? SpreadHDP { get; set; }

    public decimal? SpreadOU { get; set; }

    public decimal? SpreadXMins { get; set; }

    public int? MinorBettypeId { get; set; }

    public string ParentHashKey { get; set; }

    public string EtPk { get; set; }

    public bool IsImplicitMinorMatch { get; set; }
}