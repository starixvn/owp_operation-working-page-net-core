namespace OWP.LiveDetail.MatchSetting;

using System.Globalization;

public class UpdateCriteria : Criteria.CommandCriteria
{
    public override string GetSettingKey() => "OWP_LiveDetail_MatchSetting_Update";

    public int UserId { get; set; }

    public string SessionId { get; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    public int? MinorBettypeId { get; set; }

    public int BettypeId { get; set; }

    public string Bettype { get; set; }

    public int MatchId { get; set; }

    public decimal? Maxbet { get; set; }

    public decimal? Spread { get; set; }

    public override bool IsValid() => true;
}