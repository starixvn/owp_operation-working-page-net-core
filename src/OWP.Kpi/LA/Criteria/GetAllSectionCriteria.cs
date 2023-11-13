using Fanex.Data.Repository;
namespace OWP.Kpi.LA.Criteria;

public class GetAllSectionCriteria : CriteriaBase
{
    public int UserId { get; set; }

    public override string GetSettingKey() => "OWP_KPI_GetAllSectionInfos";

    public override bool IsValid() => true;
}