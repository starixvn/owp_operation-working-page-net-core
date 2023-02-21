using Fanex.Data.Repository;

namespace OWP.LiveDetail.Television;

public class GetAllCriteria : CriteriaBase
{
    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_TelevisionGetAll";
}