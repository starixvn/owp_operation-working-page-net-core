using Fanex.Data.Repository;

namespace OWP.AutoAssignment.Criteria.InternalApi;

public class GetLatestRequestByDateCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_AutoAssignment_GetLatestRequestIdByDate";
}