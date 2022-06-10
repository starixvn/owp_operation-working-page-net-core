using Fanex.Data.Repository;

namespace OWP.OtherSport.Submit.Criteria
{
    public class GetDeletedMatchesCriteria : CriteriaBase
    {
        public string OSMatches { get; set; }

        public override string GetSettingKey()
            => "OWP_OSAssignmentUpload_GetDeletedMatchesInfo";

        public override bool IsValid()
            => true;
    }
}