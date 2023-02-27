using Fanex.Data.Repository;

namespace OWP.LiveDetail.Provider;

public class GetAllCriteria : CriteriaBase
{
    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_Provider_GetAll";
}