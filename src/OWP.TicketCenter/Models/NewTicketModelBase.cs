using System.Collections.Generic;

namespace OWP.TicketCenter.Models
{
    public class NewTicketModelBase
    {
        // FE do not care this property
        public int TicketId { get; set; }

        public int TicketType { get; set; }

        #region message

        public string Message { get; set; }

        public int CreatorId { get; set; }

        #endregion

        public IList<InsertManyRecipientModel> Recipients { get; set; }

        public IList<FileModel> Files { get; set; } = new List<FileModel>();
    }
}
