using Fanex.Data.Repository;
namespace OWP.PreMatchReport.Criteria;

public class GetUnAssignedUserCriteria : CriteriaBase
{
    public string KeyWord { get; set; }
    public string UserIds { get; set; }
    public override bool IsValid() => true;
    public override string GetSettingKey() => "OWP_PreMatchReport_GetAllUnassignedUsers";
}
