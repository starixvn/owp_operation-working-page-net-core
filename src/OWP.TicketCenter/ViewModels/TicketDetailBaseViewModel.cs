using OWP.TicketCenter.Models;
using System.Collections.Generic;

namespace OWP.TicketCenter.ViewModels
{
    using System;
    using System.Linq;

    public class TicketDetailBaseViewModel
    {
        public int TicketId { get; set; }

        public int TicketType { get; set; }

        public string TypeName { get; set; }

        public IList<RecipientResponseModel> Recipients { get; set; }

        public IList<MessageDetailViewModel> Messages { get; set; }

        public DateTime? CommentLatestDate => Messages.FirstOrDefault()?.CreatedDate;

        public IssueCreatorModel CreatorInfo { get; set; }

        public bool IsRead { get; set; }
    }
}