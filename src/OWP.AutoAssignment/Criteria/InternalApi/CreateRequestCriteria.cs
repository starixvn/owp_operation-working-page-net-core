using Fanex.Data.Repository;
using OWP.AutoAssignment.Enum;

namespace OWP.AutoAssignment.Criteria.InternalApi
{
    public class CreateRequestCriteria : NonQueryCommand
    {
        public string RequestId { get; set; }

        public DateTime EventDate { get; set; }

        public StatusRequest Status { get; set; }

        public int CreatorId { get; set; }

        public override bool IsValid() => !string.IsNullOrEmpty(RequestId) && CreatorId > 0;

        public override string GetSettingKey() => "OWP_AutoAssignmentRequest_Insert";
    }
}