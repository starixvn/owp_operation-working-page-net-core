using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.LiveDetail;

public class GetTelevisionSuggestionCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_Television_GetSuggestion";

    public string Keyword { get; set; }

    public override bool IsValid() => true;
}