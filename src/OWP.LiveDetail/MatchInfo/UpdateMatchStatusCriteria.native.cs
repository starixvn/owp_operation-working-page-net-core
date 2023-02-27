using System.Globalization;
using OWP.Criteria;

namespace OWP.LiveDetail.MatchInfo;

public class UpdateMatchStatusCriteria : CommandCriteria
{
    public int UserId { get; set; }

    public string SessionId { get; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    public int OWPMatchId { get; set; }

    public string MatchStatus { get; set; }

    public override bool IsValid() => UserId > 0 && OWPMatchId > 0 && !string.IsNullOrWhiteSpace(MatchStatus);

    public override string GetSettingKey() => "OWP_LiveDetail_MatchInfo_UpdateMatchStatus";
}