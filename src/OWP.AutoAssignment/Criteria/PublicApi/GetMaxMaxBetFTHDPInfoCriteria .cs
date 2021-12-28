namespace OWP.AutoAssignment.Criteria.PublicApi
{
    using Fanex.Data.Repository;

    public class GetMaxMaxBetFTHDPInfoCriteria : CriteriaBase
    {
        public string MatchIds { get; set; }

        public override string GetSettingKey() => "OWP_AutoAssignment_GetMaxMaxbetForRealMatch_FTHDP";

        public override bool IsValid() => !string.IsNullOrEmpty(MatchIds);
    }
}
