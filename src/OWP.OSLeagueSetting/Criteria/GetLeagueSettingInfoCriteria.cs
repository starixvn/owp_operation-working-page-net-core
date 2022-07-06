using Fanex.Data.Repository;

namespace OWP.OSLeagueSetting.Criteria;

public class GetLeagueSettingInfoCriteria : CriteriaBase
{
    public string league { get; set; }

    public bool Status { get; set; }

    public string CategoryId { get; set; }

    public string RoundId { get; set; }

    public override string GetSettingKey() => "OWP_OSLeagueSetting_GetLeagueSettingInfo";

    public override bool IsValid() => true;
}