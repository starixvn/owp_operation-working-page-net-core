namespace OWP.EventPicking.Api.Models;

public class ExportLSTFormat: EventProductCommon
{
    [Cell("A2")]
    public int No { get; set; }

    [Cell("AI2")]
    public int? MathId => GetMatchId();

    public int InfoSiteMatchId { get; set; }

    public decimal? MaxBetHDPNumber { get; set; }

    public DateTime EventDateTime { get; set; }

    [Cell("B2")]
    public string Time => EventDateTime.ToString("hh:mm tt");

    [Cell("C2")]
    public string Agent { get; set; }

    [Cell("D2")]
    public string OtherProvider { get; set; }

    [Cell("E2")]
    public string HomeName { get; set; }

    [Cell("F2")]
    public string AwayName { get; set; }

    [Cell("G2")]
    public string LeagueName { get; set; }

    [Cell("U2")]
    public string Division { get; set; }

    [Cell("V2")]
    public string Remark2 { get; set; }

    [Cell("X2")]
    public string WB { get; set; }

    [Cell("Y2")]
    public string Round { get; set; }

    [Cell("AA2")]
    public string ETPK { get; set; }

    [Cell("AC2")]
    public string TV { get; set; }

    [Cell("AD2")]
    public string Channel { get; set; }

    [Cell("AE2")]
    public string Website1 { get; set; }

    [Cell("AF2")]
    public string Website2 { get; set; }

    [Cell("AG2")]
    public string Website3 { get; set; }

    [Cell("AH2")]
    public string Website4 { get; set; }

    [Cell("H2")] 
    public string MaxbetFormat => MaxBetHDPNumber.ToOWPMaxBetFormat(); 

    [Cell("I2")]
    public string Spread => SpreadHDP.ToOWPSpreadFormatPercent() + '-' + SpreadOU.ToOWPSpreadFormatPercent();

    public decimal? SpreadHDP { get; set; }

    public decimal? SpreadOU { get; set; }

    [Cell("J2")]
    public string NoHDP { get; set; }

    [Cell("K2")]
    public string LSTNoted { get; set; }

    public string Remark2T { get; set; }

    public string HDP { get; set; }

    public string OU { get; set; }

    public string H1ST { get; set; }

    public string H1x2 { get; set; }

    public string Min15MinOU { get; set; }

    public string Min10MinOU { get; set; }

    public string StanBy { get; set; }

    public string LiveAssistant { get; set; }

    public string LAStandby { get; set; }

    public bool IsLeagueBlackList { get; set; }

    public bool IsHomeBlackList { get; set; }

    public bool IsAwayBlackList { get; set; }

    public bool IsNoOffer { get; set; }

    public bool IsTestMatch { get; set; }

    public string RowColorHex => Misc.Constant.ColorPair.GetValueOrDefault(GetRawColor());

    public bool IsSaba => LeagueName.IsContains("SABA ");

    public int? ChildMatchId { get; set; }

    public int? MinorBettypeId { get; set; }

    public string GetRawColor()
    {
        string color = Misc.Constant.ColorWhite;
        if (MinorBettypeId.HasValue)
        {
            color = Misc.Constant.ColorYellow;
        }
        if (IsAwayBlackList || IsHomeBlackList || IsLeagueBlackList)
        {
            color = Misc.Constant.ColorGray;
        }
        if (IsNoOffer)
        {
            color = Misc.Constant.ColorRed;
        }
        return color;
    }

    public int? GetMatchId()
    {
        if (MinorBettypeId.HasValue)
        {
            return null;
        }

        return InfoSiteMatchId;
    }

    public bool IsCheck { get; set; }

}
