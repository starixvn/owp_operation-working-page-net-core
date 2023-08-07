namespace OWP.TicketCenter.Models;

public class TicketInfo
{
    public int TicketId { get; set; }

    public int TicketType { get; set; }

    public string TicketStatus { get; set; }

    public string TypeName { get; set; }

    public int? TicketSubTypeId { get; set; }

    public string TicketSubTypeName { get; set; }

    public DateTime LastCommentDate { get; set; }

    public bool IsRead { get; set; }

    public bool IsPinned { get; set; }

    public int OwpLeagueId { get; set; }

    public string OwpLeagueName { get; set; }

    public int EventType { get; set; }

    public int OwpHomeId { get; set; }

    public int OwpAwayId { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public DateTime? KoTime { get; set; }

    public string Subject { get; set; }

    public string EventName { get; set; }

    public string LeagueName { get; set; }

    public IList<MessageInfoModel> Messages { get; set; }
}