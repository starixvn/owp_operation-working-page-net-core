namespace OWP.LiveDetail.MatchInfo
{
    public class ValidationInfositeIdModel
    {
        public bool IsInfoSiteLinked { get; set; }

        public bool IsLiveStatusSynchronized { get; set; }
    }

    public class IsLiveStatusModel
    {
        public bool IsInfoSiteLinked { get; set; }

        public int? MinorBettypeId { get; set; }

        public int ParentAdminSiteId { get; set; }

        public bool IsLiveMatchStatus { get; set; }
    }
}
