namespace OWP.LiveDetail.User
{
    using Fanex.Data.Repository;

    public class GetSuggestionCriteria : CriteriaBase
    {
        private const int KeywordMinLength = 2;

        public string Keyword { get; set; }

        public override bool IsValid() => !string.IsNullOrWhiteSpace(Keyword) && Keyword.Length >= KeywordMinLength;

        public override string GetSettingKey() => "OWP_UserGetBySuggestion";
    }
}