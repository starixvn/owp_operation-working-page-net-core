namespace OWP.PreMatchNotification.Assignment.Models
{
    public class AssignmentSnapshot
    {
        public string LeagueName { get; set; }

        public int PreMatchLeagueId { get; set; }

        public bool? IsDay { get; set; }

        public string Hdp { get; set; }

        public string Ou { get; set; }

        public string HdpTakeOver { get; set; }

        public string OuTakeOver { get; set; }

        public string OtherHdp { get; set; }

        public string OtherOu { get; set; }

        public string OtherHdpTakeOver { get; set; }

        public string OtherOuTakeOver { get; set; }
    }
}
