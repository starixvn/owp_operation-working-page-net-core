using System.Collections.Generic;

namespace OWP.TicketCenter.ViewModels
{
   public  class EventInfoSender
    {
        public IList<EventInfoViewModel> EventInfoViewModel { get; set; }

        public IList<UserViewModel> UserViewModel { get; set; }
    }
}
