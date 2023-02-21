using System.Globalization;
using OWP.Criteria;

namespace OWP.LiveDetail.MatchInfo;

public class UpdateTradingInfoCriteria : CommandCriteria
{
    public int ActorId { get; set; }

    public string SessionId { get; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    public int OWPMatchId { get; set; }

    public int? NoOfHDPId { get; set; }

    public string MatchOfferIds { get; set; }

    public override bool IsValid() => ActorId > 0 && OWPMatchId > 0;

    public override string GetSettingKey() => "OWP_LiveDetail_UpdateTradingInfo";
}