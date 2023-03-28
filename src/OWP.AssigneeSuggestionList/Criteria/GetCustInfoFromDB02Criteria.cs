using Fanex.Data.Repository;

namespace OWP.AssigneeSuggestionList.Criteria;

public class GetCustInfoFromDB02Criteria : CriteriaBase

{
    public override string GetSettingKey() => "OWP_AutoAssignment_GetCustInfoFromDB02";

    public override bool IsValid() => true;
}

