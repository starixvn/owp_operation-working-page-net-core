using OWP.Extensions;
using OWP.Utils;
using System.Diagnostics.CodeAnalysis;

namespace OWP.Event.EventShared.Models;

[SuppressMessage("ReSharper", "IdentifierTypo")]
public class MatchInfo : TaskListMatchInfo
{
    public int OWPMatchId { get; set; }

    public int ParentMatchId { get; set; }

    public int? AdminSiteId { get; set; }

    public int? ParentAdminSiteId { get; set; }

    public bool IsFantasy => League.IsContains("Fantasy");

    public string IPTV { get; set; }

    public string IPTVCopy { get; set; }

    public string MatchNo { get; set; }

    public DateTime EventDate { get; set; }

    public TimeSpan KickOffTime { get; set; }

    public DateTime EventDateTime => EventDate.Add(KickOffTime);

    public TimeSpan? OtherTime { get; set; }

    public string OtherTimeDisplay => OtherTime?.ToOWPTimeFormat();

    public string Home { get; set; }

    public string Away { get; set; }

    public string League { get; set; }

    public string LeagueGroup { get; set; }

    public bool IsPES23 => League.Contains(" PES 23", StringComparison.CurrentCultureIgnoreCase);

    public bool IsPESOther =>
        League.Contains(" PES ", StringComparison.CurrentCultureIgnoreCase) &&
        !League.Contains(" PES 23", StringComparison.CurrentCultureIgnoreCase);

    public bool IsFIFA =>
        League.Contains("SABA ", StringComparison.CurrentCultureIgnoreCase) &&
        League.Contains("FIFA", StringComparison.CurrentCultureIgnoreCase);

    public string NoOfHDP { get; set; }

    public int NoOfHDPId { get; set; }

    public string Channel { get; set; }

    public string OtherProvider { get; set; }

    public string Provider { get; set; }

    public string Phone { get; set; }

    public string WatchBet { get; set; }

    public string Round { get; set; }

    public string Division { get; set; }

    public string Remark { get; set; }

    public int? MinorBettypeId { get; set; }

    public bool IsMinorMatch { get; set; }

    public bool IsOneHourToKickOffTime { get; set; }

    public bool IsAllowedToViewDetail { get; set; }

    public string MatchStatus { get; set; }

    public string MatchCode { get; set; }

    public string TeamRound { get; set; }

    public bool IsRead => TicketInfos.All(x => x.IsRead);

    public bool HasTicket => TicketInfos.Any();

    public AssignmentModel Assignment { get; set; }

    public MatchSettingModel MatchSetting { get; set; }

    public IEnumerable<Television> Televisions { get; set; }

    public IEnumerable<MatchOffer> MatchOffers { get; set; }

    public IEnumerable<TicketInfo> TicketInfos { get; set; }

    public int LengthLeague { get; set; }

    public int LengthRemark { get; set; }

    public int LengthIpTv { get; set; }

    public int LengthProvider { get; set; }

    public int LengthTraderHdp { get; set; }

    public int LengthTraderOu { get; set; }

    public int LengthOtherProductOneXTwo { get; set; }

    public int LengthOtherProduct1HOneXTwo { get; set; }

    public int LengthOtherProductFastMarket { get; set; }

    public int LengthOtherProductAllInOne { get; set; }

    public int LengthOtherProductTenMinutes { get; set; }

    public int LengthOtherProductFifteenMinutes { get; set; }

    public string KickOffTimeDisplay => KickOffTime.ToOWPTimeFormat();

    public int Rowspan
    {
        get
        {
            var rowSpan = 2;

            if (Assignment.FastMarket.Any() || MatchSetting.FastMarket.IsHasMaxbetSpread)
            {
                rowSpan++;
            }

            if (Assignment.AllInOne.Any())
            {
                rowSpan++;
            }

            if (Assignment.Specific15Mins.Any() || MatchSetting.Specific15Mins.IsHasMaxbetSpread)
            {
                rowSpan++;
            }

            if (Assignment.Specific10Mins.Any() || MatchSetting.Specific10Mins.IsHasMaxbetSpread)
            {
                rowSpan++;
            }

            return rowSpan;
        }
    }

    public int RowSpan
    {
        get
        {
            var rowSpan = 0;

            if (Assignment.FastMarket.Any() || MatchSetting.FastMarket.IsHasMaxbetSpread)
            {
                rowSpan++;
            }

            if (Assignment.AllInOne.Any())
            {
                rowSpan++;
            }

            if (Assignment.Specific15Mins.Any() || MatchSetting.Specific15Mins.IsHasMaxbetSpread)
            {
                rowSpan++;
            }

            if (Assignment.Specific10Mins.Any() || MatchSetting.Specific10Mins.IsHasMaxbetSpread)
            {
                rowSpan++;
            }

            return rowSpan;
        }
    }

    public TimeSpan? InfoSiteTime { get; set; }

    public string InfoSiteTimeDisplay { get; set; }

    public bool IsShowAbsenceIcon { get; set; }

    public string EventDateDisplay => EventDate.ToKOTimeDateFormat();

    public string ReadyDateTimeDisplay => ReadyDateTime.ToString("hh:mm tt MMM dd, yyyy");
}

public class TaskListMatchInfo
{
    public virtual bool IsCancelled { get; set; }

    public bool IsTodayEvent { get; set; }

    public bool IsReady { get; set; }

    public bool IsCompletedMatch { get; set; }

    public DateTime ReadyDateTime { get; set; }
}