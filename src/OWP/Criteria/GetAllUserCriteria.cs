namespace OWP.Criteria;
using Fanex.Data.Repository;

public class GetAllUserCriteria : CriteriaBase
{
    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_LiveDetail_User_GetAll";
}
