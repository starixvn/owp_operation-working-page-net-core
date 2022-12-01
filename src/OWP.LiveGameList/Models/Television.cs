namespace OWP.LiveGameList.Models
{
    public class Television
    {
        public int OWPMatchId { get; set; }

        public int TVId { get; set; }

        public string TVCode { get; set; }

        public string TVName { get; set; }

        public string Decoder { get; set; }

        public string Country { get; set; }

        public string Color { get; set; }

        public bool IsSelected { get; set; }
    }
}
