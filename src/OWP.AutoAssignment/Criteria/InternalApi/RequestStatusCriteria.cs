using Fanex.Data.Repository;

namespace OWP.AutoAssignment.Criteria.InternalApi;

public class RequestStatusCriteria : CriteriaBase
{
    public string RequestId { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_AutoAssignmentRequest_Api_GetStatusByRequestId";
}