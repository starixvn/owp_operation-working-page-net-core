

namespace OWP.TicketCenter.ViewModels
{
    public class MessageDetailViewModel
    {
        public int MessageId { get; set; }

        public string Message { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public int? FileId { get; set; }

        public bool IsFile => FileId.HasValue;

        public DateTime? CreatedDate { get; set; }
    }
}
