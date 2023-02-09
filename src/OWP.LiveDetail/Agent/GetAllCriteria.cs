namespace OWP.LiveDetail.Agent;

using Fanex.Data.Repository;

public class GetAllCriteria : CriteriaBase
{
    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_Agent_GetAll";
}