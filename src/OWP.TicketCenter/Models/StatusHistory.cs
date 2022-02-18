

namespace OWP.TicketCenter.Models
{
    public class StatusHistory
    {
        public int TicketId { get; set; }

        public DateTime LogDate { get; set; }

        public string ModifiedBy { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}
