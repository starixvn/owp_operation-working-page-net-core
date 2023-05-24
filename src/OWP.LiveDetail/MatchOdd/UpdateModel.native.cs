namespace OWP.LiveDetail.MatchOdd;

public class UpdateModel : CommandModel
{
    public int Id { get; set; }

    public List<MatchOddUpdateRow> RecommendOdds { get; set; }
}