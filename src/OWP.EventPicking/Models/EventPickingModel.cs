using System.Globalization;
using System.Text.Json.Serialization;
using Epplus.Mapper.Annotations;
using OWP.EventPicking.Misc;
using OWP.Extensions;
using OWP.Utils;

namespace OWP.EventPicking.Models;

public class EventPickingModel
{
    public int InfoSiteMatchId { get; set; }

    public int? ChildMatchId { get; set; }

    public int? AdminSiteMatchId { get; set; }

    public int? MinorBettypeId { get; set; }

    public int? SubType { get; set; }

    public bool IsChildMatch => SubType.HasValue;

    public bool IsTestMatch { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsCheck { get; set; }

    public bool IsSaba => LeagueName.IsContains("SABA ");

    public DateTime EventDateTime { get; set; }

    public int OWPLeagueId { get; set; }

    public bool IsBlackList { get; set; }

    [Cell("A2")]
    public string MatchNo { get; set; }

    [Cell("B2")]
    public string MatchStatus => IsNoOffer ? "Cancel" : string.Empty;

    [Cell("C2")]
    public string EventDate => EventDateTime.Date.ToString(CultureInfo.InvariantCulture);

    [Cell("D2")]
    public string Time => EventDateTime.ToString("hh:mm tt");

    [Cell("E2")]
    public string OtherTime { get; set; }

    [Cell("F2")]
    public string HomeName { get; set; }

    [Cell("G2")]
    public string AwayName { get; set; }

    [Cell("H2")]
    public string LeagueName { get; set; }

    [Cell("I2")]
    public string MatchOffer { get; set; }

    [Cell("J2")]
    public string LstNote { get; set; }

    [Cell("K2")]
    public string MaxBetHDP { get; set; }

    [Cell("L2")]
    public string SpreadHDP { get; set; }

    [Cell("M2")]
    public string SpreadOU { get; set; }

    [Cell("N2")]
    public string NoHDP { get; set; }

    [Cell("O2")]
    public string TraderHDP { get; set; }

    [Cell("P2")]
    public string TraderOU { get; set; }

    [Cell("Q2")]
    public string Trader1HHDP { get; set; }

    [Cell("R2")]
    public string Trader1HOU { get; set; }

    [Cell("S2")]
    public string OtherTradersHDP { get; set; }

    [Cell("T2")]
    public string OtherTradersOU { get; set; }

    [Cell("U2")]
    public string OtherTraders1HHDP { get; set; }

    [Cell("V2")]
    public string OtherTraders1HOU { get; set; }

    [Cell("W2")]
    public string X15mins { get; set; }

    [Cell("X2")]
    public string X10mins { get; set; }

    [Cell("Y2")]
    public string Trader1X2 { get; set; }

    [Cell("Z2")]
    public string OtherTraders1X2 { get; set; }

    [Cell("AA2")]
    public string FM { get; set; }

    [Cell("AB2")]
    public string AIO { get; set; }

    [Cell("AC2")]
    public string LA { get; set; }

    [Cell("AD2")]
    public string OtherLA { get; set; }

    [Cell("AE2")]
    public string LASystemAssign { get; set; }

    [Cell("AF2")]
    public string Provider { get; set; }

    [Cell("AG2")]
    public string OtherProvider { get; set; }

    [Cell("AH2")]
    public string Division { get; set; }

    [Cell("AI2")]
    public string Remark { get; set; }

    [Cell("AJ2")]
    public string Wb { get; set; }

    [Cell("AK2")]
    public string Round { get; set; }

    [Cell("AL2")]
    public string TV { get; set; }

    [Cell("AM2")]
    public string Channel { get; set; }

    [Cell("AN2")]
    public string Iptv { get; set; }

    [Cell("AO2")]
    public string Agent { get; set; }

    [Cell("AP2")]
    public string ReporterPhone { get; set; }

    [Cell("AQ2")]
    public string InfoSiteIdExport => IsChildMatch ? string.Empty : InfoSiteMatchId.ToString();

    [Cell("AR2")]
    public string AdminSiteIdExport => IsChildMatch ? string.Empty : AdminSiteMatchId.ToString();

    [Cell("AS2")]
    public string Venue { get; set; }

    [Cell("AT2")]
    public string HomeRemark { get; set; }

    [Cell("AU2")]
    public string AwayRemark { get; set; }

    [Cell("AV2")]
    public string ETPK { get; set; }

    [Cell("AW2")]
    public string RemarkFromInfosite { get; set; }

    [Cell("AX2")]
    public string Remark2 { get; set; }

    [Cell("AY2")]
    public string TotalRound { get; set; }

    [Cell("AZ2")]
    public string Website1 { get; set; }

    [Cell("BA2")]
    public string Website2 { get; set; }

    [Cell("BB2")]
    public string Website3 { get; set; }

    [Cell("BC2")]
    public string Website4 { get; set; }

    [Cell("BD2")]
    public string OtherTraders15mins { get; set; }

    [Cell("BE2")]
    public string OtherTraders10mins { get; set; }

    [Cell("BF2")]
    public string OtherTradersFM { get; set; }

    [Cell("BG2")]
    public string OtherTradersAIO { get; set; }

    [Cell("BH2")]
    public string MaxBetOU { get; set; } = string.Empty;

    [Cell("BI2")]
    public string MaxBet1HHDP { get; set; } = string.Empty;

    [Cell("BJ2")]
    public string MaxBet1HOU { get; set; } = string.Empty;

    [Cell("BK2")]
    public string Spread1HHDP { get; set; } = string.Empty;

    [Cell("BL2")]
    public string Spread1HOU { get; set; } = string.Empty;

    [Cell("BM2")]
    public string MaxBetFT1X2 { get; set; } = string.Empty;

    [Cell("BN2")]
    public string Spread1X2 { get; set; } = string.Empty;

    [Cell("BO2")]
    public string MaxBet1H1X2 { get; set; } = string.Empty;

    [Cell("BP2")]
    public string Spread1H1X2 { get; set; } = string.Empty;

    [Cell("BQ2")]
    public string Trader1H1X2 { get; set; } = string.Empty;

    [Cell("BR2")]
    public string OtherTraders1H1X2 { get; set; }

    [Cell("BS2")]
    public string MaxBetXmins { get; set; }

    [Cell("BT2")]
    public string SpreadXmins { get; set; }

    [Cell("BU2")]
    public string MaxBetFM { get; set; }

    [Cell("BV2")]
    public string SpreadFM { get; set; }

    [JsonIgnore]
    public bool HomeUpdated { get; set; }

    [JsonIgnore]
    public bool AlwayUpdated { get; set; }

    public bool IsNoOffer { get; set; }

    public string RowColorHex => Misc.Constant.ColorPair.GetValueOrDefault(RowColor);

    public string RowColor { get; set; }

    public int LengthStreamingCell { get; set; }

    public int LengthProviderCell { get; set; }

    public int LengthRoundCell { get; set; }

    public int LengthRemarkCell { get; set; }

    public bool Confirmed { get; set; }

    // Main match
    public string LiveTV { get; set; }

    public string OutSide { get; set; }

    public string LocalTV { get; set; }

    // MaxBet & Spread
    public decimal? MaxBetHDPNumber { get; set; }

    public List<RAndDInfo> RAndDInfos { get; set; }

    public string HasProvider => !string.IsNullOrWhiteSpace(Provider)
    ? "yes"
    : "no";

    public string HasOtherProvider => !string.IsNullOrWhiteSpace(OtherProvider)
    ? "yes"
    : "no";

    public void MapWeekendSettings(LeagueSetting leagueSetting)
    {
        MatchOffer = leagueSetting.MaxbetWK_MatchOffer;
        LstNote = leagueSetting.FormatedLSTNote;
        MaxBetHDPNumber = leagueSetting.MaxbetWK_HDP;
        MaxBetHDP = leagueSetting.MaxbetWK_HDP.ToOWPMaxBetFormat();
        SpreadHDP = leagueSetting.Spread_HDP.ToOWPSpreadFormatPercent();
        NoHDP = leagueSetting.MaxbetWK_NoOfHDP;
        MaxBetOU = leagueSetting.MaxbetWK_OU.ToOWPMaxBetFormat();
        SpreadOU = leagueSetting.Spread_OU.ToOWPSpreadFormatPercent();
        MaxBet1HHDP = leagueSetting.MaxbetWK_1HHDP.ToOWPMaxBetFormat();
        MaxBet1HOU = leagueSetting.MaxbetWK_1HOU.ToOWPMaxBetFormat();
        Spread1HHDP = leagueSetting.Spread_1HHDP.ToOWPSpreadFormatPercent();
        Spread1HOU = leagueSetting.Spread_1HOU.ToOWPSpreadFormatPercent();
        MaxBetFT1X2 = leagueSetting.MaxBet_FT1X2.ToOWPMaxBetFormat();
        Spread1X2 = leagueSetting.MaxBet_1X2Spread.ToOWPSpreadFormatPercent();
        MaxBet1H1X2 = leagueSetting.MaxBet_1H1X2.ToOWPMaxBetFormat();
        Spread1H1X2 = leagueSetting.Spread_1H1X2.ToOWPSpreadFormatPercent();
    }

    public void MapWeekdaySettings(LeagueSetting leagueSetting)
    {
        MatchOffer = leagueSetting.MaxbetWD_MatchOffer;
        LstNote = leagueSetting.FormatedLSTNote;
        MaxBetHDPNumber = leagueSetting.MaxbetWD_HDP;
        MaxBetHDP = leagueSetting.MaxbetWD_HDP.ToOWPMaxBetFormat();
        SpreadHDP = leagueSetting.Spread_HDP.ToOWPSpreadFormatPercent();
        MaxBetOU = leagueSetting.MaxbetWD_OU.ToOWPMaxBetFormat();
        SpreadOU = leagueSetting.Spread_OU.ToOWPSpreadFormatPercent();
        NoHDP = leagueSetting.MaxbetWD_NoOfHDP;
        MaxBet1HHDP = leagueSetting.MaxbetWD_1HHDP.ToOWPMaxBetFormat();
        MaxBet1HOU = leagueSetting.MaxbetWD_1HOU.ToOWPMaxBetFormat();
        Spread1HHDP = leagueSetting.Spread_1HHDP.ToOWPSpreadFormatPercent();
        Spread1HOU = leagueSetting.Spread_1HOU.ToOWPSpreadFormatPercent();
        MaxBetFT1X2 = leagueSetting.MaxBet_FT1X2.ToOWPMaxBetFormat();
        Spread1X2 = leagueSetting.MaxBet_1X2Spread.ToOWPSpreadFormatPercent();
        MaxBet1H1X2 = leagueSetting.MaxBet_1H1X2.ToOWPMaxBetFormat();
        Spread1H1X2 = leagueSetting.Spread_1H1X2.ToOWPSpreadFormatPercent();

    }
}