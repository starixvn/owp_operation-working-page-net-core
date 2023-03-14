using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.PreMatchAssignment;

class GetLastUpdatedPreMatchAssignmentCriteria : CriteriaBase
{
    public bool IsDay { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_GetLastUpdated";

    public override bool IsValid() => true;
}