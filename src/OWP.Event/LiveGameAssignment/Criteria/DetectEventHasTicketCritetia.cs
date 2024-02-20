using Fanex.Data.Repository;

namespace OWP.Event.LiveGameAssignment.Criteria;

public class DetectEventHasTicketCritetia : CriteriaBase
{
    public string OWPMatchIds { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_TicketEvent_DetectEventHasTicket";

    public override bool IsValid() => true;
}
