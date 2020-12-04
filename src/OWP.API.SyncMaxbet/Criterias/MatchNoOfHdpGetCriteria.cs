namespace OWP.API.SyncMaxbet.Criterias
{
    using Fanex.Data.Repository;

    public class MatchNoOfHdpGetCriteria : CriteriaBase
    {
        public int OWPMatchId { get; set; }

        public override string GetSettingKey() => "OWP_API_SyncMaxbet_GetMatchNoHDP";

        public override bool IsValid() => OWPMatchId > 0;
    }
}