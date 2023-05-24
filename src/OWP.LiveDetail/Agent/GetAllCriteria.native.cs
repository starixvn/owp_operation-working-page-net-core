using Fanex.Data.Repository;

namespace OWP.LiveDetail.Agent;

public class GetAllCriteria : CriteriaBase
{
    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_Agent_GetAll";
}