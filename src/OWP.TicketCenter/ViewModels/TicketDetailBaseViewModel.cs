using System.Collections.Generic;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.ViewModels
{
    public class TicketDetailBaseViewModel
    {
        public int TicketId { get; set; }

        public int TicketType { get; set; }

        public string TypeName { get; set; }

        public IList<RecipientResponseModel> Recipients { get; set; }

        public IList<MessageDetailViewModel> Messages { get; set; }

        public IssueCreatorModel CreatorInfo
        {
            get;
            set;
        }
    }
}
