namespace OWP.AutoAssignment.Criteria.InternalApi
{
    using Fanex.Data.Repository;

    public class GetProviderInfoCriteria : CriteriaBase
    {
        public string MatchIds { get; set; }

        public string RequestId { get; set; }

        public override string GetSettingKey() => "OWP_AutoAssignment_GetProviderInfoByIds";

        public override bool IsValid()
            => !string.IsNullOrEmpty(MatchIds)
               && !string.IsNullOrEmpty(RequestId);
    }
}