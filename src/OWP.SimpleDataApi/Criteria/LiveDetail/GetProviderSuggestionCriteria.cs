namespace OWP.SimpleDataApi.Criteria;

using Fanex.Data.Repository;

public class GetProviderSuggestionCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_Provider_GetSuggestion";

    public string Keyword { get; set; }

    public override bool IsValid() => true;
}