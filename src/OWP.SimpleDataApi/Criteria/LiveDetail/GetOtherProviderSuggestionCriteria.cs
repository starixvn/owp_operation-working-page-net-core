using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.LiveDetail;

public class GetOtherProviderSuggestionCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_OtherProvider_GetSuggestion";

    public string Keyword { get; set; }

    public override bool IsValid() => true;
}