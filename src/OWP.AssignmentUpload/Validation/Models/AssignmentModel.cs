using System.ComponentModel.DataAnnotations;

namespace OWP.AssignmentUpload.Validation.Models;

public class AssignmentModel
{
    public int OWPMatchId { get; set; }

    public bool IsNeutral { get; set; }

    public string HashKey { get; set; } = string.Empty;

    public string SessionId { get; set; } = string.Empty;

    public DateTime EventDate { get; set; }

    public string MatchStatus { get; set; } = string.Empty;

    public int InfoSiteMatchId { get; set; }

    public int AdminSiteId { get; set; }

    public string MatchNo { get; set; } = string.Empty;

    public TimeSpan KickOffTime { get; set; }

    public TimeSpan? OtherTime { get; set; }

    public string Home { get; set; } = string.Empty;

    public string Away { get; set; } = string.Empty;

    public string League { get; set; } = string.Empty;

    public bool IsSaba { get; set; }

    public string Agent { get; set; } = string.Empty;

    public string Channel { get; set; } = string.Empty;

    public string IPTV { get; set; } = string.Empty;

    public string Division { get; set; } = string.Empty;

    public string MatchOffer { get; set; } = string.Empty;

    public string NoHDP { get; set; } = string.Empty;

    public List<string> Notes { get; set; } = [];

    public string OtherProvider { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Provider { get; set; } = string.Empty;

    public string Remark { get; set; } = string.Empty;

    public string AgentEst { get; set; } = string.Empty;

    public string RemarkEst { get; set; } = string.Empty;

    public string RemarkBy { get; set; } = string.Empty;

    public string Round { get; set; } = string.Empty;

    public string TV { get; set; } = string.Empty;

    public string WB { get; set; } = string.Empty;

    public string Website1 { get; set; } = string.Empty;

    public string Website2 { get; set; } = string.Empty;

    public string Website3 { get; set; } = string.Empty;

    public string Website4 { get; set; } = string.Empty;

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

    public string OtherTraders1H1X2 { get; set; } = string.Empty;

    public string OtherTraders10Mins { get; set; } = string.Empty;

    public string OtherTraders15Mins { get; set; } = string.Empty;

    public string OtherTraders1HHDP { get; set; } = string.Empty;

    public string OtherTraders1HOU { get; set; } = string.Empty;

    public string OtherTraders1X2 { get; set; } = string.Empty;

    public string OtherTradersAIO { get; set; } = string.Empty;

    public string OtherTradersFM { get; set; } = string.Empty;

    public string OtherTradersHDP { get; set; } = string.Empty;

    public string OtherTradersOU { get; set; } = string.Empty;

    public string OtherTradersLA { get; set; } = string.Empty;

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

    public string ParentHashKey { get; set; } = string.Empty;

    public string EtPk { get; set; }

    public bool IsImplicitMinorMatch { get; set; }

    public string AIOBT { get; set; }

    public string SOP { get; set; }
}