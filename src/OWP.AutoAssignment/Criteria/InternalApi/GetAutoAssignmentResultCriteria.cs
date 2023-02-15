namespace OWP.AutoAssignment.Criteria.InternalApi;

using Fanex.Data.Repository;

public class GetAutoAssignmentResultCriteria : CriteriaBase
{
    public string RequestId { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_AutoAssignment_GetInfoSiteMatchResultByRequestId";
}