namespace OWP.PrematchAssignment.Criteria
{
    using Fanex.Data.Repository;

    public class QueryAssigneePreMatchByPreLeagueIdCriteria: CriteriaBase
    {
        public int Id { get; set; }

        public override string GetSettingKey()
            => "OWP_PreMatchAssignment_GetAssigneeByPreMatchLeagueId";

        public override bool IsValid() => true;
    }
}
