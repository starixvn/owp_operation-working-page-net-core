
using Fanex.Data.Repository;

namespace OWP.PreMatchAssignment.Criteria
{
    class InsertNewIssueCriteria : NonQueryCommand
    {
        public int ActorId { get; set; }

        public string Message { get; set; }

        public int? OWPLeagueId { get; set; }

        public int? InfoSiteMatchId { get; set; }

        public int? FileId { get; set; }

        public override string GetSettingKey() => "OWP_PreMatchAssignment_InsertNewIssue";

        public override bool IsValid() => ActorId > 0 && (OWPLeagueId > 0 || InfoSiteMatchId > 0);
    }
}
