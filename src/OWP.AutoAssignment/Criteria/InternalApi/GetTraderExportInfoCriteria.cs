namespace OWP.AutoAssignment.Criteria.InternalApi
{
    public class GetTraderExportInfoCriteria : ExportInfoCriteriaBase
    {
        public override string GetSettingKey()
            => "OWP_AutoAssignment_Export_GetTraderInfo";

        public override bool IsValid()
            => true;
    }
}