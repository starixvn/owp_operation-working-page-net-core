namespace OWP.Event.LiveGameAssignment.Models;

using Extensions;
using System;
using System.Collections.Generic;

public class EventModel
{
    public int OWPMatchId { get; set; }

    public int ParentMatchId { get; set; }

    public int? AdminSiteId { get; set; }

    public bool IsFantasy => League.IsContains("Fantasy");

    public int? MinorBettypeId { get; set; }

    public string MatchNo { get; set; }

    public string MatchStatus { get; set; }

    public string NoOfHDP { get; set; }

    public string NoOfHDPShort { get; set; }

    public int NoOfHDPId { get; set; }

    public DateTime EventDateTime { get; set; }

    public string KickOffTime { get; set; }

    public string OtherTime { get; set; }

    public string League { get; set; }

    public bool IsSaBa => League.IsContains("SABA");

    public string LeagueGroup { get; set; }

    public string Home { get; set; }

    public string Away { get; set; }

    public string Channel { get; set; }

    public string Remark { get; set; }

    public string Round { get; set; }

    public string Division { get; set; }

    public string OtherProvider { get; set; }

    public string Provider { get; set; }

    public string Agent { get; set; }

    public string WatchBet { get; set; }

    public string Phone { get; set; }

    public string Website1 { get; set; }

    public string Website2 { get; set; }

    public string Website3 { get; set; }

    public string Website4 { get; set; }

    public int? InfoSiteMatchId { get; set; }

    public bool IsInfoSiteLinked { get; set; }

    public bool IsLiveStatusSynchronized { get; set; }

    public string MatchCode { get; set; }

    public string TeamRound { get; set; }

    public bool? HasTicket { get; set; }

    public bool? IsRead { get; set; }

    public IList<MatchNote> Notes { get; set; }

    public IList<Television> Televisions { get; set; }

    public IList<MatchOffer> MatchOffers { get; set; }

    public List<MatchSetting> MatchSettings { get; set; } = [];

    public MatchSetting HandicapSetting { get; set; }

    public MatchSetting OverUnderSetting { get; set; }

    public IList<Assignment> OneXTwo { get; set; } = new List<Assignment>();

    public IList<Assignment> FM { get; set; } = new List<Assignment>();

    public IList<Assignment> AIO { get; set; } = new List<Assignment>();

    public IList<Assignment> Handicap { get; set; } = new List<Assignment>();

    public IList<Assignment> Handicap1st { get; set; } = new List<Assignment>();

    public IList<Assignment> OverUnder { get; set; } = new List<Assignment>();

    public IList<Assignment> OverUnder1st { get; set; } = new List<Assignment>();

    public IList<Assignment> OneXTwo1st { get; set; } = new List<Assignment>();

    public IList<Assignment> Specific15Mins { get; set; } = new List<Assignment>();

    public IList<Assignment> Specific10Mins { get; set; } = new List<Assignment>();

    public IList<Assignment> LA { get; set; } = new List<Assignment>();

    public bool IsAllowedToViewDetail { get; set; }

    public string IPTVCopy { get; set; }

    public TimeSpan? InfoSiteTime { get; set; }

    public string InfoSiteTimeDisplay { get; set; }

    public bool IsShowAbsenceIcon { get; set; }

    public TimeSpan KickOffTimeStr { get; set; }

    public TimeSpan? OtherTimeStr { get; set; }

    public string AIOName { get; set; }

    public string AIOShortName { get; set; }
    
    public string SOP { get; set; }
}