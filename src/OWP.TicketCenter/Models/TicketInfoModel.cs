namespace OWP.TicketCenter.Models
{
    public class TicketBaseInfo
    {
        public int TicketId { get; set; }

        public int TicketType { get; set; }

        public string TicketStatus { get; set; }

        public string TypeName { get; set; }

        public int? TicketSubTypeId { get; set; }

        public string TicketSubTypeName { get; set; }

        public DateTime? LastCommentDate { get; set; }

        public bool IsRead { get; set; }

        public bool IsPinned { get; set; }

        public IList<MessageInfoModel> Messages { get; set; }
    }

    public class TicketLeagueInfo : TicketBaseInfo
    {
        public int OwpLeagueId { get; set; }

        public string OwpLeagueName { get; set; }

        public int EventType { get; set; } = 1;

        public string EventName => OwpLeagueName;
    }

    public class TicketMatchInfo : TicketBaseInfo
    {
        public int OwpLeagueId { get; set; }

        public string OwpLeagueName { get; set; }

        public int OwpHomeId { get; set; }

        public int OwpAwayId { get; set; }

        public string HomeName { get; set; }

        public string AwayName { get; set; }

        public DateTime? KOTime { get; set; }

        public int EventType { get; set; } = 2;

        public string EventName => OwpLeagueName;
    }

    public class TicketOtherInfo : TicketBaseInfo
    {
        public string Subject { get; set; }

        public int EventType { get; set; } = 3;

        public string EventName => Subject;
    }

    public class TicketSupportInfo : TicketBaseInfo
    {
        public string Subject { get; set; }

        public int EventType { get; set; } = 4;

        public string EventName => Subject;
    }
}