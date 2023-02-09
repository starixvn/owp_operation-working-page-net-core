namespace OWP.AutoAssignment.Criteria.InternalApi;

using Fanex.Data.Repository;

public class GetRequestsCriteria : CriteriaBase
{
    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_AutoAssignmentRequest_GetAll";
}