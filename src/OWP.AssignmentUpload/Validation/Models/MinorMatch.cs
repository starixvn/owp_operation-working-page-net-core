namespace OWP.AssignmentUpload.Validation.Models
{
    public class MinorMatch
    {
        public int MinorBettypeId { get; set; }

        public string LeagueLike { get; set; }

        public string LeagueNotLike { get; set; }

        public string HomeLike { get; set; }

        public string HomeNotLike { get; set; }

        public string AwayLike { get; set; }

        public string AwayNotLike { get; set; }

        public int LeagueChars { get; set; }

        public int HomeChars { get; set; }

        public int AwayChars { get; set; }
    }
}
