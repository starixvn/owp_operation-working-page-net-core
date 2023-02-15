namespace OWP.LiveDetail.MatchOdd;

public class CreateCriteria : CommandCriteria
{
    public int Id { get; set; }

    public int OWPMatchId { get; set; }

    public int Bettype { get; set; }

    public float Handicap { get; set; }

    public int TeamChoice { get; set; }

    public string RecommendOdds { get; set; }

    public override bool IsValid() => base.IsValid() && OWPMatchId > 0 && Bettype > 0 && !string.IsNullOrWhiteSpace(RecommendOdds);

    public override string GetSettingKey() => "OWP_LiveDetail_InsertRecommendOdds";
}