using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;
public class GetUserFeedbackInfoCriteria : CriteriaBase
{
    public int sessionId { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_GetUserFeedbackInfo";

    public override bool IsValid() => sessionId > 0;
}
