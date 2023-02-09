namespace OWP.LiveDetail.MatchInfo;

using System;
using System.Globalization;
using OWP.Criteria;

public class UpdateMatchInfoCriteria : CommandCriteria
{
    public int ActorId { get; set; }

    public string SessionId { get; private set; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    public int OWPMatchId { get; set; }

    public bool IsSelected { get; set; }

    public long? InfoSiteMatchId { get; set; }

    public string MatchStatus { get; set; }

    public string Remark { get; set; }

    public string Round { get; set; }

    public bool IsUpdateRound { get; set; }

    public string Division { get; set; }

    public bool IsUpdateDivision { get; set; }

    public string Venue { get; set; }

    public bool IsUpdateVenue { get; set; }

    public DateTime? OtherTime { get; set; }

    public bool IsUpdateOtherTime { get; set; }

    public string Market { get; set; }

    public bool IsUpdateMarket { get; set; }

    public string Website1 { get; set; }

    public bool IsUpdateWebsite1 { get; set; }

    public string Website2 { get; set; }

    public bool IsUpdateWebsite2 { get; set; }

    public string Website3 { get; set; }

    public bool IsUpdateWebsite3 { get; set; }

    public string Website4 { get; set; }

    public bool IsUpdateWebsite4 { get; set; }

    public override bool IsValid() => ActorId > 0 && OWPMatchId > 0 && !string.IsNullOrWhiteSpace(MatchStatus);

    public override string GetSettingKey() => "OWP_LiveDetail_Update_MatchInfo";
}