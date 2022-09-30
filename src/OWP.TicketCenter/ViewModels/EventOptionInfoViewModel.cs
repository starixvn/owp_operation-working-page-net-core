namespace OWP.TicketCenter.ViewModels
{
    public class EventOptionInfoViewModel
    {
        public int OwpLeagueId { get; set; }

        public string LeagueName { get; set; }

        public int? HomeId { get; set; }

        public int? AwayId { get; set; }

        public string HomeName { get; set; }

        public string AwayName { get; set; }

        public DateTime? EventDateTimeGmt8 { get; set; }

        public bool IsLeague { get; set; }
    }

    public class EventCheckingOptionInfoViewModel : EventOptionInfoViewModel
    {
        public int InfoSiteMatchId { get; set; }
    }

    public class EventLGAOptionInfoViewModel : EventOptionInfoViewModel
    {
        public int OwpMatchId { get; set; }
    }
}