using Fanex.Data.Repository;

namespace OWP.LeagueManagement.Criteria;

public class SpecialProductGetLogCriteria : CriteriaBase
{
    public int ObjectId { get; set; }

    public int PageSize { get; set; }

    public override string GetSettingKey() => "OWP_SpecialProduct_GetLog";

    public override bool IsValid() => ObjectId > 0 && PageSize > 0;
}