namespace OWP.LiveDetail.MatchOdd
{
    using System;
    using System.Globalization;

    public class UpdateCriteria : OWP.Criteria.CommandCriteria
    {
        public int UserId { get; set; }

        public int Id { get; set; }

        public float Handicap { get; set; }

        public int TeamChoice { get; set; }

        public string SessionId { get; private set; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

        public string RecommendOdds { get; set; }

        public override bool IsValid() => UserId > 0 && Id > 0 && !string.IsNullOrWhiteSpace(RecommendOdds);

        public override string GetSettingKey() => "OWP_LiveDetail_UpdateRecommendOdds";
    }
}