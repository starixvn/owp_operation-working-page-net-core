namespace OWP.Event.LiveGameAssignment.Download.Models;

using System;

public class MatchInfo
{
    public int OWPMatchId { get; set; }

    public TimeSpan KickOffTime { get; set; }

    public TimeSpan? OtherTime { get; set; }

    public string League { get; set; }

    public string Home { get; set; }

    public string Away { get; set; }

    public string NoOfHDP { get; set; }

    public string MatchNo { get; set; }

    public string TV { get; set; }

    public string Channel { get; set; }

    public string IPTV { get; set; }

    public string WatchBet { get; set; }

    public string Agent { get; set; }

    public string AgentEst { get; set; }

    public string RemarkEst { get; set; }

    public string RemarkBy { get; set; }

    public string OtherProvider { get; set; }

    public string Provider { get; set; }

    public string Phone { get; set; }

    public string Round { get; set; }

    public string TeamRound { get; set; }

    public string OutSide { get; set; }

    public string Remark { get; set; }

    public string MatchOffer { get; set; }

    public string Website { get; set; }

    public string Website2 { get; set; }

    public string Website3 { get; set; }

    public string Website4 { get; set; }

    public bool IsMinorMatch { get; set; }

    public int? MinorBettypeId { get; set; }

    public int ParentMatchId { get; set; }

    public string MatchStatus { get; set; }

    public int? InfoSiteMatchId { get; set; }

    public int? MatchId { get; set; }

    public bool IsLive { get; set; }

    public int ParentAdminSiteId { get; set; }

    public DateTime EventDateTime
    {
        get; set;
    }
    public string AIOBT
    {
        get; set;
    }
    public string SOP
    {
        get; set;
    }
}
