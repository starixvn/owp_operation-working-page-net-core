namespace OWP.SimpleDataApi.Criteria.LiveGameAssignment;

using Fanex.Data.Repository;

public class GetSpecialEventTemplateCriteria : CriteriaBase
{
    public int ParentMatchId { get; set; }

    public int MinorBettypeId { get; set; }

    public override string GetSettingKey() => "OWP_LiveGameAssignment_GetSpecialEventTemplate";

    public override bool IsValid() => ParentMatchId > 0 && MinorBettypeId != 0;
}