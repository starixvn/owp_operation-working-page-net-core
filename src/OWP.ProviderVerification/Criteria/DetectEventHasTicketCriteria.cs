using Fanex.Data.Repository;

namespace OWP.ProviderVerification.Criteria;

public class DetectEventHasTicketCriteria : CriteriaBase
{
    public string OWPMatchIds { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_TicketEvent_DetectEventHasTicket";

    public override bool IsValid() => true;
}