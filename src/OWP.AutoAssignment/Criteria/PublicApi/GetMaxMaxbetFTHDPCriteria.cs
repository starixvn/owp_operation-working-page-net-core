using Fanex.Data.Repository;

namespace OWP.AutoAssignment.Criteria.PublicApi;

public class GetMaxMaxbetFTHDPCriteria : CriteriaBase
{
    public string MatchIds { get; set; }

    public override string GetSettingKey() => "OWP_AutoAssignment_GetMaxMaxbet_FTHDP";

    public override bool IsValid() => !string.IsNullOrEmpty(MatchIds);
}