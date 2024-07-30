namespace OWP.SimpleDataApi.Criteria.LiveGameAssignment;

using Fanex.Data.Repository;

public class GetBettypeForTradingInfoCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_LiveGameAssignment_GetBettypeForTradingInfo";

    public override bool IsValid() => true;
}
