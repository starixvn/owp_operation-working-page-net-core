namespace OWP.Event.LiveGameAssignment.Download.Models;

using System.ComponentModel;

public enum ColumnName
{
    [Description("No")]
    No,

    [Description("Match Status")]
    MatchStatus,

    [Description("Time")]
    Time,

    [Description("Other Time")]
    OtherTime,

    [Description("Home")]
    Home,

    [Description("Away")]
    Away,

    [Description("League")]
    League,

    [Description("Match Offer")]
    MatchOffer,

    [Description("LST Note")]
    LSTNote,

    [Description("Note")]
    Note,

    [Description("Max Bet (HDP)")]
    MaxBet_HDP,

    [Description("Spread (HDP)")]
    Spread_HDP,

    [Description("Spread (O/U)")]
    Spread_OU,

    [Description("No.HDP")]
    NoOfHDP,

    [Description("Trader (HDP)")]
    Trader_HDP,

    [Description("Trader (O/U)")]
    Trader_OU,

    [Description("Trader (1H HDP)")]
    Trader_1HHDP,

    [Description("Trader (1H O/U)")]
    Trader_1HOU,

    [Description("Other Traders (HDP)")]
    OtherTrader_HDP,

    [Description("Other Traders (O/U)")]
    OtherTrader_OU,

    [Description("Other Traders (1H HDP)")]
    OtherTrader_1HHDP,

    [Description("Other Traders (1H O/U)")]
    OtherTrader_1HOU,

    [Description("15mins")]
    Trader_15Mins,

    [Description("10mins")]
    Trader_10Mins,

    [Description("Trader (1X2)")]
    Trader_1X2,

    [Description("Other Traders (1X2)")]
    OtherTrader_1X2,

    [Description("FM")]
    Trader_FM,

    [Description("AIO")]
    Trader_AIO,

    [Description("LA")]
    LA,

    [Description("Other LA")]
    OtherLA,

    [Description("Provider")]
    Provider,

    [Description("Other Provider")]
    OtherProvider,

    [Description("Division")]
    Division,

    [Description("Remark")]
    Remark,

    [Description("Streaming Info(WB)")]
    StreamingInfoWB,

    [Description("Round")]
    Round,

    [Description("Round (H-A/T)")]
    TeamRound,

    [Description("TV")]
    TV,

    [Description("Channel")]
    Channel,

    [Description("IPTV")]
    IPTV,

    [Description("Agent")]
    Agent,

    [Description("Agent for EST")]
    AgentEst,

    [Description("Remark for EST")]
    RemarkEst,

    [Description("Remark By")]
    RemarkBy,

    [Description("Reporter Phone")]
    ReporterPhone,

    [Description("InfoSite ID")]
    InfositeId,

    [Description("AdminSite ID")]
    AdminSiteId,

    [Description("Web 1")]
    Web1,

    [Description("Web 2")]
    Web2,

    [Description("Web 3")]
    Web3,

    [Description("Web 4")]
    Web4,

    [Description("Other Traders (15 mins)")]
    OtherTrader_15Mins,

    [Description("Other Traders (10 mins)")]
    OtherTrader_10Mins,

    [Description("Other Traders (FM)")]
    OtherTrader_FM,

    [Description("Other Traders (AIO)")]
    OtherTrader_AIO,

    [Description("Max Bet (O/U)")]
    MaxBet_OU,

    [Description("Max Bet (1H HDP)")]
    MaxBet_1HHDP,

    [Description("Max Bet (1H O/U)")]
    MaxBet_1HOU,

    [Description("Spread (1H HDP)")]
    Spread_1HHDP,

    [Description("Spread (1H O/U)")]
    Spread_1HOU,

    [Description("Max Bet (1X2)")]
    MaxBet_1X2,

    [Description("Spread (1X2)")]
    Spread_1X2,

    [Description("Max Bet (1H 1X2)")]
    MaxBet_1H1X2,

    [Description("Spread (1H 1X2)")]
    Spread_1H1X2,

    [Description("Trader (1H 1X2)")]
    Trader_1H1X2,

    [Description("Other Traders (1H 1X2)")]
    OtherTrader_1H1X2,

    [Description("Max Bet (x mins)")]
    MaxBet_XMins,

    [Description("Spread (x mins)")]
    Spread_XMins,

    [Description("Max Bet (FM)")]
    MaxBet_FM,

    [Description("Spread (FM)")]
    Spread_FM,
    
    [Description("AIO BT")]
    AIOBT,

    [Description("SOP")]
    SOP
}