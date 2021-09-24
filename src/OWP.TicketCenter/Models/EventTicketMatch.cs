namespace OWP.TicketCenter.Models
{
    public  class EventTicketMatch
    {
        public int OWPLeagueId { get; set; }

        public string OWPLeagueName { get; set; }

        public int OwpHomeId { get; set; }

        public int OwpAwayId { get; set; }

        public string HomeName { get; set; }

        public string AwayName { get; set; }
    }
}
