using Fanex.Data.Repository;

namespace OWP.Criteria;

public class GetAllUserCriteria : CriteriaBase
{
    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey()
    {
        return "OWP_LiveDetail_User_GetAll";
    }
}