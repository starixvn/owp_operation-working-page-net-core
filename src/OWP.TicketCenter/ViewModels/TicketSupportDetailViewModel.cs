namespace OWP.TicketCenter.ViewModels
{
    public class TicketSupportDetailViewModel : TicketDetailBaseViewModel
    {
        public string Subject { get; set; }

        public string EventName => Subject;
    }
}