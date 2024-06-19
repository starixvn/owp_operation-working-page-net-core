using Fanex.Data.Repository;

namespace OWP.EventPicking.Api.Criteria;

public class InsertOrDeleteNoOfferMatchCriteria : NonQueryCommand
{
    public string MatchInfoJson { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_InsertOrDeleteNoOfferMatch";

    public override bool IsValid() => true;
}
