using System.ComponentModel;

namespace OWP.Event.EventShared.Enum;

public enum MajorBetType
{
    [Description("Handicap")]
    Handicap = 1,

    [Description("Over/Under")]
    OverUnder = 3,

    [Description("FT.1X2")]
    OneXTwo = 5,

    [Description("1H Handicap")]
    Handicap1St = 7,

    [Description("1H Over/Under")]
    OverUnder1St = 8,

    [Description("1H 1X2")]
    OneXTwo1St = 15,

    [Description("XMins")]
    XMins = -1001,

    [Description("2H LA")]
    La2Nd = -1002,

    [Description("FM")]
    FastMarket = -1003,

    [Description("AIO")]
    AllInOne = -1004
}
