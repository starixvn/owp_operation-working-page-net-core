using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.PreMatchAssignment;

public class QueryNoteMessagesCriteria : CriteriaBase
{
    public int PreMatchNoteId { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_QueryNoteMessages";

    public override bool IsValid() => PreMatchNoteId > 0;
}