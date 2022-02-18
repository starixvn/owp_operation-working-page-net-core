namespace OWP.TicketCenter.Models
{
    public class EventTicketModelBase
    {
        public int TicketId { get; set; }

        public int TicketType { get; set; }

        public string TicketStatus { get; set; }

        public string TypeName { get; set; }

        #region message

        public int UserId { get; set; }

        public string Username { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string Message { get; set; }

        public int MessageId { get; set; }

        public int? FileId { get; set; }

        #endregion message

        public bool IsRead { get; set; }

        public bool IsPinned { get; set; }
    }
}