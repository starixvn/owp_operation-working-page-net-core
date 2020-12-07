namespace OWP.API.SyncMaxbet.Criterias
{
    using Fanex.Data.Repository;

    public class MaxbetGetCriteria : CriteriaBase
    {
        public int OWPMatchId { get; set; }

        public bool IsCN88 { get; set; }

        public override string GetSettingKey() => "OWP_API_SyncMaxbet_GetMaxbet";

        public override bool IsValid() => OWPMatchId > 0;
    }
}