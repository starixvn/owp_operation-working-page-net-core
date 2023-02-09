namespace OWP.LiveGameList.Criteria;

using Fanex.Data.Repository;

public class GetMinorAdminSiteIdsCriteria : CriteriaBase
{
    public string MinorMatches { get; set; }

    public override string GetSettingKey() => "OWP_LiveGameList_GetMinorAdminSiteMatchIds";

    public override bool IsValid()
        => true;
}