using OWP.Extensions;

namespace OWP.LiveGameList.Models
{
    public class MatchInfo
    {
        public int OWPMatchId { get; set; }

        public int ParentMatchId { get; set; }

        public int? AdminSiteId { get; set; }

        public bool IsFantasy => League.IsContains("Fantasy");

        public string IPTV { get; set; }

        public string IPTVCopy { get; set; }

        public string MatchNo { get; set; }

        public DateTime EventDate { get; set; }

        public TimeSpan KickOffTime { get; set; }

        public DateTime EventDateTime => EventDate.Add(KickOffTime);

        public TimeSpan? OtherTime { get; set; }

        public string Home { get; set; }

        public string Away { get; set; }

        public string League { get; set; }

        public bool IsPES
        {
            get => League.IndexOf(" PES ", StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        public bool IsFIFA
        {
            get => League.IndexOf("SABA ", StringComparison.CurrentCultureIgnoreCase) >= 0 &&
                   League.IndexOf("FIFA", StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        public string NoOfHDP { get; set; }

        public int NoOfHDPId { get; set; }

        public string Channel { get; set; }

        public string OtherProvider { get; set; }

        public string Provider { get; set; }

        public string Phone { get; set; }

        public string WatchBet { get; set; }

        public string Round { get; set; }

        public string Division { get; set; }

        public string Remark { get; set; }

        public int? MinorBettypeId { get; set; }

        public bool IsMinorMatch { get; set; }

        public bool IsOneHourToKickOffTime { get; set; }

        public bool IsAllowedToViewDetail { get; set; }

        public string MatchStatus { get; set; }

        public string MatchCode { get; set; }

        public bool IsRead { get; set; }

        public bool HasTicket { get; set; }

        public AssignmentModel Assignment { get; set; }

        public MatchSettingModel MatchSetting { get; set; }

        public IEnumerable<Television> Televisions { get; set; }

        public IEnumerable<MatchOffer> MatchOffers { get; set; }

        public IEnumerable<TicketInfo> TicketInfos { get; set; }

        public int LengthLeague { get; set; }

        public int LengthRemark { get; set; }

        public int LengthFtTraderHdp { get; set; }

        public int LengthFtTraderOu { get; set; }

        public int Length1HTraderHdp { get; set; }

        public int Length1HTraderOu { get; set; }

        public int LengthLa { get; set; }

        public int LengthIpTv { get; set; }

        public int LengthProvider { get; set; }
    }

}
