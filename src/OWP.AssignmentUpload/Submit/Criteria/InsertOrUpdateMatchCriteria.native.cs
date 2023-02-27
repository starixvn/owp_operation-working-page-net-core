using Fanex.Data.Repository;

namespace OWP.AssignmentUpload.Submit.Criteria;

public class InsertOrUpdateMatchCriteria : CriteriaBase
{
    public string Assignments { get; set; }

    public DateTime EventDate { get; set; }

    public int UserId { get; set; }

    public override string GetSettingKey() => "OWP_AssignmentUpload_MatchInsertOrUpdate";

    public override bool IsValid() => true;
}