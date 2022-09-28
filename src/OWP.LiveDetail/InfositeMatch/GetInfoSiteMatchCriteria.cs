namespace OWP.LiveDetail.InfositeMatch
{
    using System;
    using Fanex.Data.Repository;

    public class GetInfoSiteMatchCriteria : CriteriaBase
    {
        public int InfoSiteMatchId { get; set; }

        public DateTime EventDate { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "OWP_LiveGameAssignment_GetInfoSiteMatch";

        public override bool IsValid() => InfoSiteMatchId > 0;
    }
}