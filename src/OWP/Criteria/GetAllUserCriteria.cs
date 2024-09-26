using Fanex.Data.Repository;

namespace OWP.Criteria;

public class GetAllUserCriteria : CriteriaBase
{
    public string AllUserJson { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_LiveDetail_User_GetAll";

}