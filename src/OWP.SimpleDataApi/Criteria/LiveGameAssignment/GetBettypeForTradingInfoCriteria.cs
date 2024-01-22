namespace OWP.SimpleDataApi.LiveGameAssignment.Criteria;

using Fanex.Data.Repository;

public class GetBettypeForTradingInfoCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_LiveGameAssignment_GetBettypeForTradingInfo";

    public override bool IsValid() => true;
}
