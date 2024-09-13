using Fanex.Data.Repository;

namespace OWP.Api.Trader.Criteria;

public class GetTraderByAdminsiteMatchIdsCriteria : CriteriaBase
{
    public string MatchesJson { get; set; }

    public override string GetSettingKey() => "OWP_TraderApi_GetTraderByAdminsiteMatchIds";

    public override bool IsValid() => true;
}