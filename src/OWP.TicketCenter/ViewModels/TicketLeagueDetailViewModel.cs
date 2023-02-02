namespace OWP.TicketCenter.ViewModels
{
    public class TicketLeagueDetailViewModel : TicketDetailBaseViewModel
    {
        public int OwpLeagueId { get; set; }

        public string OwpLeagueName { get; set; }

        public string EventName => OwpLeagueName;

        public int EventType => (int)Enum.EventType.League;
    }
}