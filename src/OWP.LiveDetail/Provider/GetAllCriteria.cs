namespace OWP.LiveDetail.Provider;

using Fanex.Data.Repository;

public class GetAllCriteria : CriteriaBase
{
    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_Provider_GetAll";
}