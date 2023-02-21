using Fanex.Data.Repository;

namespace OWP.LiveDetail.User;

public class GetUserListCriteria : CriteriaBase
{
    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_LiveDetail_User_GetAll";
}