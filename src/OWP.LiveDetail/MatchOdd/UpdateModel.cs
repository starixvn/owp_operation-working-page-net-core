namespace OWP.LiveDetail.MatchOdd
{
    using System.Collections.Generic;

    public class UpdateModel : CommandModel
    {
        public int Id { get; set; }

        public List<MatchOddUpdateRow> RecommendOdds { get; set; }
    }
}