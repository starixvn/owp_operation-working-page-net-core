using Fanex.Data.Repository;
namespace OWP.TicketCenter.Criteria;
public class GetFunctionFeedbackCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_Feedback_GetFunctionFeedback";

    public override bool IsValid() => true;
}
