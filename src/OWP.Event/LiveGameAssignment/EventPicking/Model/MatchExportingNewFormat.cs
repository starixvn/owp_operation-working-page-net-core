using Epplus.Mapper.Annotations;
using Olympus.InfoSiteClient.Models;
using OWP.Event.LiveGameAssignment.Download.Converter;
using OWP.Event.LiveGameAssignment.EventPicking.Builder;
using OWP.Event.LiveGameAssignment.EventPicking.Service;
using OWP.Extensions;
using OWP.Utils;

namespace OWP.Event.LiveGameAssignment.EventPicking.Model;

public class MatchExportingNewFormat
{
    private static readonly DateTime MinDate = new DateTime(2000, 1, 1);

    public void SetMatchInfo(EventInfo eventInfo)
    {
        MatchNo = eventInfo.MatchNo;

        MatchStatus = eventInfo.MatchStatus;

        OtherTime = eventInfo.OtherTime.HasValue ? eventInfo.OtherTime.Value.ToOWPTimeFormat() : string.Empty;

        TraderHDP = eventInfo.TraderHDP;

        TraderOU = eventInfo.TraderOU;

        Trader1HHDP = eventInfo.Trader1HHDP;

        Trader1HOU = eventInfo.Trader1HOU;

        OtherTradersHDP = eventInfo.OtherTradersHDP;

        OtherTradersOU = eventInfo.OtherTradersOU;

        OtherTraders1HHDP = eventInfo.OtherTraders1HHDP;

        OtherTraders1HOU = eventInfo.OtherTraders1HOU;

        X15mins = eventInfo.X15mins;

        X10mins = eventInfo.X10mins;

        Trader1X2 = eventInfo.Trader1X2;

        OtherTraders1X2 = eventInfo.OtherTraders1X2;

        FM = eventInfo.FM;

        AIO = eventInfo.AIO;

        LA = eventInfo.LA;

        OtherLA = eventInfo.OtherLA;

        LASystemAssign = eventInfo.LASystemAssign;

        Remark = eventInfo.Remark;

        ReporterPhone = eventInfo.ReporterPhone;

        OtherTraders15mins = eventInfo.OtherTraders15mins;

        OtherTraders10mins = eventInfo.OtherTraders10mins;

        OtherTradersFM = eventInfo.OtherTradersFM;

        OtherTradersAIO = eventInfo.OtherTradersAIO;

        Trader1H1X2 = eventInfo.Trader1H1X2;

        OtherTraders1H1X2 = eventInfo.OtherTraders1H1X2;

        MaxBetXmins = eventInfo.MaxBetXmins;

        SpreadXmins = eventInfo.SpreadXmins;

        MaxBetFM = eventInfo.MaxBetFM;

        SpreadFM = eventInfo.SpreadFM;
    }

    public void MapInfoSiteToMainMatch(CommonMatchEx infositeMatch)
    {
        var extInfo = infositeMatch.ext_info ?? new SoccerEventSchedule();
        InfoSiteID = infositeMatch.match_id;
        AdminSiteID = CommonBuilder.BuildMatchId(infositeMatch);
        Venue = extInfo.venue;
        HomeRemark = extInfo.home_remark;
        AwayRemark = extInfo.away_remark;
        EtPK = extInfo.et_pk;
        Remark = extInfo.remark;
        Remark2 = extInfo.remark2;
        TotalRound = extInfo.round_total;
        website1 = extInfo.website1;
        website2 = extInfo.website2;
        website3 = extInfo.website3;
        website4 = extInfo.website4;
        LeagueName = infositeMatch.league_name;
        HomeName = LeagueHomeAwayBuilder.BuildHomeName(infositeMatch, extInfo);
        AwayName = LeagueHomeAwayBuilder.BuildAwayName(infositeMatch, extInfo);
        Round = StringExtension.ConvertNullToEmpty(extInfo.round);
        TV = StringExtension.ConvertNullToEmpty(extInfo.local_tv);
        LocalTV = StringExtension.ConvertNullToEmpty(extInfo.local_tv);
        LiveTV = StringExtension.ConvertNullToEmpty(extInfo.tv_live);
        Channel = StringExtension.ConvertNullToEmpty(extInfo.tv_live);
        Wb = WatchBetBuilder.Build(extInfo);
        Iptv = StringExtension.ConvertNullToEmpty(extInfo.ip_tv);
        Agent = StringExtension.ConvertNullToEmpty(extInfo.agent);
        Agent = StringExtension.ConvertNullToEmpty(extInfo.agent);
        Venue = StringExtension.ConvertNullToEmpty(extInfo.venue);
        Provider = ProviderBuilder.Build(infositeMatch.ext_info?.provider_use);
        OtherProvider = OtherProviderBuilder.Build(infositeMatch);
        Division = StringExtension.ConvertNullToEmpty(infositeMatch.ext_info?.code);
        GameDate = CommonBuilder.BuildGameDate(infositeMatch);
        Agent = extInfo.agent;
        website1 = extInfo.website1;
        website2 = extInfo.website2;
        website3 = extInfo.website3;
        website4 = extInfo.website4;
        game_date = extInfo.game_date.ToString();
        OutSide = StringExtension.ConvertNullToEmpty(extInfo.code);
    }

    public void MapInfoSiteToChildMatch(ChildMatch childMatch)
    {
        InfoSiteID = childMatch.match_id;
        AdminSiteID = unchecked((int)childMatch.create_match_id.GetValueOrDefault(0));
        LeagueName = childMatch.league_name;
        HomeName = childMatch.home_name;
        AwayName = childMatch.away_name;
        Wb = string.Empty;
        Round = string.Empty;
        Agent = string.Empty;
        Venue = string.Empty;
        Provider = string.Empty;
        LocalTV = string.Empty;
        LiveTV = string.Empty;
        GameDate = DateTimeConverter.ConvertTimeToUTC8(childMatch.game_date.GetValueOrDefault(MinDate));
        Agent = string.Empty;
        website1 = string.Empty;
        website2 = string.Empty;
        website3 = string.Empty;
        website4 = string.Empty;
        game_date = GameDate.ToString();
        OutSide = string.Empty;
    }

    public bool HasProvider
        => string.IsNullOrWhiteSpace(StringExtension.RemoveNonAlphaCharacters(Provider));

    public bool HasBlacklist
        => IsBlackList;

    public bool HasMaxbet
        => string.IsNullOrWhiteSpace(MaxBetHDP);

    public decimal? MaxBetHDPNumber { get; set; }

    public string RowColor { get; set; }

    public string LiveTV { get; set; }

    public string OutSide { get; set; }

    public string LocalTV { get; set; }

    public DateTime GameDate { get; set; }

    public bool IsBlackList { get; set; }

    public string game_date { get; set; }

    [Cell("A2")]
    public string MatchNo { get; set; }

    [Cell("B2")]
    public string MatchStatus { get; set; }

    [Cell("C2")]
    public string EventDate => GameDate.Date.ToString();

    [Cell("D2")]
    public string Time => GameDate.ToString("hh:mm tt");

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

    public long InfoSiteID { get; set; }

    public int? AdminSiteID { get; set; }

    [Cell("AQ2")]
    public string InfoSiteIdExport => CommonBuilder.BuildInfositeIdExport(HomeName, InfoSiteID);

    [Cell("AR2")]
    public string AdminSiteIdExport => AdminSiteID.GetValueOrDefault() == 0 ?
        string.Empty : AdminSiteID.ToString();

    [Cell("AS2")]
    public string Venue { get; set; }

    [Cell("AT2")]
    public string HomeRemark { get; set; }

    [Cell("AU2")]
    public string AwayRemark { get; set; }

    [Cell("AV2")]
    public string EtPK { get; set; }

    [Cell("AW2")]
    public string RemarkFromInfosite { get; set; }

    [Cell("AX2")]
    public string Remark2 { get; set; }

    [Cell("AY2")]
    public string TotalRound { get; set; }

    [Cell("AZ2")]
    public string website1 { get; set; }

    [Cell("BA2")]
    public string website2 { get; set; }

    [Cell("BB2")]
    public string website3 { get; set; }

    [Cell("BC2")]
    public string website4 { get; set; }

    [Cell("BD2")]
    public string OtherTraders15mins { get; set; }

    [Cell("BE2")]
    public string OtherTraders10mins { get; set; }

    [Cell("BF2")]
    public string OtherTradersFM { get; set; }

    [Cell("BG2")]
    public string OtherTradersAIO { get; set; }

    [Cell("BH2")]
    public string MaxBetOU { get; set; }

    [Cell("BI2")]
    public string MaxBet1HHDP { get; set; }

    [Cell("BJ2")]
    public string MaxBet1HOU { get; set; }

    [Cell("BK2")]
    public string Spread1HHDP { get; set; }

    [Cell("BL2")]
    public string Spread1HOU { get; set; }

    [Cell("BM2")]
    public string MaxBetFT1X2 { get; set; }

    [Cell("BN2")]
    public string Spread1X2 { get; set; }

    [Cell("BO2")]
    public string MaxBet1H1X2 { get; set; }

    [Cell("BP2")]
    public string Spread1H1X2 { get; set; }

    [Cell("BQ2")]
    public string Trader1H1X2 { get; set; }

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
}
