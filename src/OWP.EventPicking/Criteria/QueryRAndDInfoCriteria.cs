

using Fanex.Data.Repository;

namespace OWP.EventPicking.Criteria;
public class QueryRAndDInfoCriteria : CriteriaBase
{
    public DateTime Date { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_GetRandDInfo";

    public override bool IsValid() => true;
}
