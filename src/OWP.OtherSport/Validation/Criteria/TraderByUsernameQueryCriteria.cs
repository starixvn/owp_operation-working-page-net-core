using Fanex.Data.Repository;

namespace OWP.OtherSport.Validation.Criteria;

public class TraderByUsernameQueryCriteria : CriteriaBase
{
    public string Username { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_OSAssignmentUpload_GetTraderByUsername";
}