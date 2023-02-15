namespace OWP.OSLeagueSetting.Models;

using System.Xml.Serialization;

[XmlRoot(ElementName = "z")]
public class SettingModelDeserialize
{
    [XmlAttribute("osRound")] public string Round { get; set; }

    [XmlAttribute("osWeekMatch")] public string WeekMatch { get; set; }

    [XmlAttribute("osOfferLines")] public string OfferLines { get; set; }

    [XmlAttribute("osBetDelay")] public string BetDelay { get; set; }

    [XmlAttribute("osProvider")] public string Provider { get; set; }

    [XmlAttribute("osKOFromTime")] public string KOFromTime { get; set; }

    [XmlAttribute("osKOToTime")] public string KOToTime { get; set; }

    [XmlAttribute("osSpreadHDP")] public string SpreadHDP { get; set; }

    [XmlAttribute("osSpreadOU")] public string SpreadOU { get; set; }

    [XmlAttribute("osMaxBetHDP")] public string MaxBetHDP { get; set; }

    [XmlAttribute("osMaxBetOU")] public string MaxBetOU { get; set; }

    [XmlAttribute("osRemark")] public string Remark { get; set; }

    [XmlAttribute("osIsManualUpdateScore")]
    public string IsManualUpdateScore { get; set; }

    [XmlAttribute("osIsIpTvOnly")] public string IsIpTvOnly { get; set; }

    [XmlAttribute("osBetType")] public string BetType { get; set; }
}

[XmlRoot(ElementName = "z")]
public class LeagueInfoModelDeserialize
{
    [XmlAttribute("osLeagueName")] public string LeagueName { get; set; }

    [XmlAttribute("osIsActive")] public string IsActive { get; set; }

    [XmlAttribute("osRegion")] public string Region { get; set; }

    [XmlAttribute("osCategory")] public string Category { get; set; }
}