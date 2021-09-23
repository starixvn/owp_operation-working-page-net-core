using System.Collections.Generic;

namespace OWP.TicketCenter.ViewModels
{
   public  class EventOptionInfoSender
    {
        public IList<EventInfoViewModel> EventInfoViewModel { get; set; }

        public IList<UserViewModel> UserViewModel { get; set; }
    }
}
