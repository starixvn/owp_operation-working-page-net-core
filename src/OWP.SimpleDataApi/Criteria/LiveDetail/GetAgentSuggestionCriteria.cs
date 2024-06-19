namespace OWP.SimpleDataApi.Criteria.LiveDetail;

public class GetAgentSuggestionCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_Agent_GetSuggestion";

    public string Keyword { get; set; }

    public override bool IsValid() => true;
}