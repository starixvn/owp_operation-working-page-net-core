using Fanex.Data.Repository;

namespace OWP.LiveDetail.MatchInfo;

public class ValidatePhoneCriteria : CriteriaBase
{
    public int OWPMatchId { get; set; }

    public bool IsValidateAssignment { get; set; }

    public string Phone { get; set; }

    public override bool IsValid() => OWPMatchId > 0;

    public override string GetSettingKey() => "OWP_ValidationRule_CheckWhenEdit";
}