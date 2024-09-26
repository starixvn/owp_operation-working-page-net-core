using Fanex.Data.Repository;

namespace OWP.Kpi.LA.Criteria;

public class CalculateKpiCriteria : CriteriaBase
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string SectionIds { get; set; }

    public bool ActiveUser { get; set; }

    public bool ResignUser { get; set; }

    public string AllUserJson { get; set; }

    public override string GetSettingKey() => "OWP_Kpi_Get";

    public override bool IsValid() => true;
}