namespace OWP.LiveDetail.MatchOdd
{
    using System.Collections.Generic;

    public class CreateModel : CommandModel
    {
        public List<MatchOddUpdateRow> RecommendOdds { get; set; }
    }
}