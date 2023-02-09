namespace OWP.AutoAssignment.Criteria.InternalApi;

public class GetLAExportInfoCriteria : ExportInfoCriteriaBase
{
    public override string GetSettingKey() => "OWP_AutoAssignment_Export_GetLAInfo";

    public override bool IsValid() => true;
}