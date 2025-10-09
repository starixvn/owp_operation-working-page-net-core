namespace OWP.Event.LiveGameAssignment.Models;

using System;

public class MatchInfo
{
    public int OWPMatchId { get; set; }

    public string MatchNo { get; set; }

    public DateTime EventDateTime { get; set; }

    public TimeSpan KickOffTime { get; set; }

    public TimeSpan? OtherTime { get; set; }
    public string LeagueGroup { get; set; }

    public string League { get; set; }

    public string Home { get; set; }

    public string Away { get; set; }

    public string NoOfHDP { get; set; }

    public int NoOfHDPId { get; set; }

    public string Channel { get; set; }

    public string WatchBet { get; set; }

    public string Agent { get; set; }

    public string OtherProvider { get; set; }

    public string Provider { get; set; }

    public string Phone { get; set; }

    public string Round { get; set; }

    public string OutSide { get; set; }

    public string Remark { get; set; }

    public bool IsMinorMatch { get; set; }

    public int? MinorBettypeId { get; set; }

    public int ParentMatchId { get; set; }

    public string MatchStatus { get; set; }

    public int? InfoSiteMatchId { get; set; }

    public int ParentAdminSiteId { get; set; }

    public int SubType { get; set; }

    public bool IsInfoSiteLinked { get; set; }

    public bool IsLiveMatchStatus { get; set; }

    public string Website1 { get; set; }

    public string Website2 { get; set; }

    public string Website3 { get; set; }

    public string Website4 { get; set; }

    public bool IsAllowedToViewDetail { get; set; } = true;

    public string IpTv { get; set; }

    public string MatchCode { get; set; }

    public string TeamRound { get; set; }

    public bool IsShowAbsenceIcon { get; set; }

    public string AIO { get; set; }

    public string SOP { get; set; }
}