using Fanex.Data.Repository;

namespace OWP.AutoAssignment.Criteria.InternalApi;

public class GetLeagueSettingInfoCriteria : CriteriaBase
{
    public string MatchIds { get; set; }

    public string RequestId { get; set; }

    public override string GetSettingKey() => "OWP_AutoAssignment_GetLeagueSettingInfo";

    public override bool IsValid()
        => !string.IsNullOrEmpty(MatchIds)
           && !string.IsNullOrEmpty(RequestId);
}