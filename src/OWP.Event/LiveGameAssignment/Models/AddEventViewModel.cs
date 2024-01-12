namespace OWP.Event.LiveGameAssignment.Models;

using System;
using System.Collections.Generic;

public class AddEventViewModel
{
    public int? ParentMatchId { get; set; }

    public DateTime EventDate { get; set; }

    public string MatchStatus { get; set; }

    public string MatchNo { get; set; }

    public int? InfoSiteId { get; set; }

    public string LeagueName { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public TimeSpan KOTime { get; set; }

    public TimeSpan? OtherTime { get; set; }

    public string Remark { get; set; }

    public string Round { get; set; }

    public string Division { get; set; }

    public string Website1 { get; set; }

    public string Website2 { get; set; }

    public string Website3 { get; set; }

    public string Website4 { get; set; }

    public string TVIds { get; set; }

    public string Channel { get; set; }

    public string WB { get; set; }

    public string Provider { get; set; }

    public string OtherProvider { get; set; }

    public string AgentIds { get; set; }

    public string Phone { get; set; }

    public int? NoOfHDPId { get; set; }

    public string MatchOfferIds { get; set; }

    public List<MatchSettingModel> MatchSettings { get; set; }

    public List<MatchOddViewModel> MatchOdds { get; set; }
}