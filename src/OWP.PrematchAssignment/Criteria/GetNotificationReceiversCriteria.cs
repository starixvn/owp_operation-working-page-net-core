using Fanex.Data.Repository;

namespace OWP.PreMatchAssignment.Criteria
{

    public class GetNotificationReceiversCriteria : CriteriaBase
    {
        public int ActorId { get; set; }

        public int PreMatchIssueId { get; set; }

        public override string GetSettingKey() => "OWP_PreMatchAssignment_GetIssueNotificationReceivers";

        public override bool IsValid() => PreMatchIssueId > 0 && ActorId > 0;
    }
}