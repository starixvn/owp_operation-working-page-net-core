namespace OWP.AutoAssignment.Criteria.PublicApi;

using Fanex.Data.Repository;

public class GetCustInfoFromDB02Criteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_AutoAssignment_GetCustInfoFromDB02";

    public override bool IsValid() => true;
}