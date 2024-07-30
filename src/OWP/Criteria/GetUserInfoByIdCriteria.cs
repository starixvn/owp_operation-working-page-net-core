using Fanex.Data.Repository;

namespace OWP.Criteria;

public class GetUserInfoByIdCriteria
    : CriteriaBase
{
    public int UserId { get; set; }

    public override bool IsValid()
    {
        return UserId > 0;
    }

    public override string GetSettingKey()
    {
        return "OWP_GetUserInfoById";
    }
}