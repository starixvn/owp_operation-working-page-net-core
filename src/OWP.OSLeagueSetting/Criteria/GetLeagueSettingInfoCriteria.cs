namespace OWP.OSLeagueSetting.Criteria;

using Fanex.Data.Repository;

public class GetLeagueSettingInfoCriteria : CriteriaBase
{
    public string League { get; set; }

    public string Status { get; set; }

    public string CategoryId { get; set; }

    public string RoundId { get; set; }

    public override string GetSettingKey() => "OWP_OSLeagueSetting_GetLeagueSettingInfo";

    public override bool IsValid() => true;
}