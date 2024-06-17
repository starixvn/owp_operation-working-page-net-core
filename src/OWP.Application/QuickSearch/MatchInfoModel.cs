
using System.Globalization;


namespace OWP.Application.QuickSearch;
public class MatchInfoModel
{
    public int RowNumber { get; set; }

    public int OWPMatchId { get; set; }

    public int? ParentMatchId { get; set; }

    public string MatchNo { get; set; }

    public string Event { get; set; }

    public int? Bettype { get; set; }

    public DateTime EventDate { get; set; }

    public string LeagueName { get; set; }

    public string EventDateString => $"{EventDate.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture)} - {EventDate.ToString("hh:mm tt", CultureInfo.InvariantCulture)}";

    public string KickOffTime => EventDate.ToString("hh:mm tt", CultureInfo.InvariantCulture);

    public bool IsToday { get; set; }

    public bool IsAllowedToViewDetail { get; set; }

    public bool IsHasAssignee { get; set; }

    public string Provider { get; set; }

    public int? AdminSiteId { get; set; }

    public string IPTV { get; set; }

    public string WatchBet { get; set; }

    public string Channel { get; set; }

    public string IPTVCopy { get; set; }

    public AssignmentInfo MainTrader { get; set; }

    public AssignmentInfo OtherTrader { get; set; }

    public AssignmentInfo MainLA { get; set; }

    public AssignmentInfo OtherLA { get; set; }
}