namespace OWP.LiveGameList.Models
{
    public class MinorMatch
    {
        public int OwpMatchId { get; set; }

        public int? ParentMatchId { get; set; }

        public int? SubType { get; set; }
    }

    public class MinorMatchInfo
    {
        public int? OwpMatchId { get; set; }

        public int? AdminMatchId { get; set; }
    }
}
