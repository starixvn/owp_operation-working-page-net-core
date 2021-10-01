using System;

namespace OWP.TicketCenter.Models
{
    public class InsertMessageResultModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string NickName { get; set; }

        public DateTime CreatedDate { get; set; }

        public int MessageId { get; set; }

        public string Message { get; set; }
    }
}
