using Fanex.Data.Repository;

namespace OWP.LiveGameList.Criteria
{
    public class GetMinorAdminSiteIdsCriteria : CriteriaBase
    {
        public string MinorMatches { get; set; }

        public override string GetSettingKey() => "OWP_LiveGameList_GetMinorAdminSiteMatchIds";

        public override bool IsValid()
            => true;
    }
}
