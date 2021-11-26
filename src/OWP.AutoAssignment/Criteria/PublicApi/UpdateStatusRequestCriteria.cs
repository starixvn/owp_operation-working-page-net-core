using Fanex.Data.Repository;
using OWP.AutoAssignment.Enum;

namespace OWP.AutoAssignment.Criteria.PublicApi
{
    public class UpdateStatusRequestCriteria : NonQueryCommand
    {
        public string RequestId { get; set; }

        public StatusRequest Status { get; set; }

        public override string GetSettingKey() => "OWP_AutoAssignmentRequest_Api_UpdateStatusRequestId";

        public override bool IsValid() => true;
    }
}