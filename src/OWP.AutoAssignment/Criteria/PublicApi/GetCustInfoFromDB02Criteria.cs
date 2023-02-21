using Fanex.Data.Repository;

namespace OWP.AutoAssignment.Criteria.PublicApi;

public class GetCustInfoFromDB02Criteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_AutoAssignment_GetCustInfoFromDB02";

    public override bool IsValid() => true;
}