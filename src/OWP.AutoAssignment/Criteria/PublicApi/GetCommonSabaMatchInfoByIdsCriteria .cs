namespace OWP.AutoAssignment.Criteria.PublicApi
{
    using Fanex.Data.Repository;

    internal class GetCommonSabaMatchInfoByIdsCriteria : CriteriaBase
    {
        public string Ids { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => "OWP_AutoAssignment_GetCommonSabaMatchInfoByIds";
    }
}
