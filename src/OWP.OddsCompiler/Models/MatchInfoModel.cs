using OWP.Extensions;
using System;
using System.Collections.Generic;

namespace OWP.OddsCompiler.Models;

public class MatchInfoModel
{
    public int OWPMatchId { get; set; }

    public string MatchNo { get; set; }

    public int? InfoSiteID { get; set; }

    public string MatchOffer { get; set; }

    public DateTime EventDate { get; set; }

    public TimeSpan KickOffTime { get; set; }

    public string MatchName => Home + " -vs- " + Away;

    public string Home { get; set; }

    public string Away { get; set; }

    public string League { get; set; }

    public bool IsSelected { get; set; }

    public string NoOfHDP { get; set; }

    public string Round { get; set; }

    public bool IsSoloMatch { get; set; }

    public string OutSide { get; set; }

    public string ExtraTimeRemark { get; set; }

    public string Venue { get; set; }

    public string TVCode { get; set; }

    public string LocalTV { get; set; }

    public string LiveTV { get; set; }

    public string Channel { get; set; }

    public string WatchBet { get; set; }

    public string Agent { get; set; }

    public string OtherProvider { get; set; }

    public string Provider { get; set; }

    public string Phone { get; set; }

    public string Remark { get; set; }

    public string Division { get; set; }

    public string Market { get; set; }

    public string Website1 { get; set; }

    public string Website2 { get; set; }

    public string Website3 { get; set; }

    public string Website4 { get; set; }

    public bool IsLinked { get; set; }

    public List<MatchSetting> MatchSettings { get; set; } = [];

    public MatchOddsRow MatchOddsRow { get; set; } = new();

    public string KickOffTimeToDisplay => KickOffTime.ToKOTime();

    public int? ParentMatchId { get; set; }

    public TimeSpan? OtherTime { get; set; }

    public TimeSpan? InfoSiteTime { get; set; }

    public string InfoSiteTimeDisplay { get; set; }

    public string OtherTimeToDisplay => OtherTime.HasValue ? OtherTime.Value.ToKOTime() : string.Empty;

    public string MatchStatus { get; set; }
}