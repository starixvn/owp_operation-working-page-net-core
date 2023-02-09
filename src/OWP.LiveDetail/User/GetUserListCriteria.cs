namespace OWP.LiveDetail.User;

using Fanex.Data.Repository;

public class GetUserListCriteria : CriteriaBase
{
    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_LiveDetail_User_GetAll";
}