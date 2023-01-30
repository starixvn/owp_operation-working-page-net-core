using Newtonsoft.Json;
using OWP.Utils;

namespace OWP.TaskList.Models
{

    public class MatchInfo
    {
        public MatchInfo()
        {
            Assignment = new AssignmentModel();
            MatchSetting = new MatchSettingModel();
            Televisions = new List<Television>();
        }

        public int OWPMatchId { get; set; }

        public int ParentMatchId { get; set; }

        public int? AdminSiteId { get; set; }

        public bool IsFantasy => League.Contains("Fantasy", StringComparison.OrdinalIgnoreCase);

        public string MatchNo { get; set; }

        public DateTime EventDate { get; set; }

        public TimeSpan KickOffTime { get; set; }

        public TimeSpan? OtherTime { get; set; }

        public string Home { get; set; }

        public string Away { get; set; }

        public string MatchCode { get; set; }

        public string League { get; set; }

        public bool IsPES => League.IndexOf(" PES ", StringComparison.CurrentCultureIgnoreCase) >= 0;

        public bool IsFIFA => League.IndexOf("SABA ", StringComparison.CurrentCultureIgnoreCase) >= 0 &&
                   League.IndexOf("FIFA", StringComparison.CurrentCultureIgnoreCase) >= 0;

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

        public string IPTV { get; set; }

        public string IPTVCopy { get; set; }

        public bool IsMinorMatch
        {
            get { return OWPMatchId != ParentMatchId; }
        }

        public bool IsOneHourToKickOffTime { get; set; }

        public bool IsAllowedToViewDetail { get; set; }

        public virtual bool IsCancelled { get; set; }

        public string MatchStatus { get; set; }

        public bool IsTodayEvent { get; set; }

        public bool IsReady { get; set; }

        public bool IsCompletedMatch { get; set; }

        public DateTime ReadyDateTime { get; set; }

        public AssignmentModel Assignment { get; set; }

        public MatchSettingModel MatchSetting { get; set; }

        public IEnumerable<Television> Televisions { get; set; }

        public IEnumerable<MatchOffer> MatchOffers { get; set; }

        public IEnumerable<TicketInfo> TicketInfos { get; set; }

        public bool IsRead { get; set; }

        public bool HasTicket { get; set; }

        public DateTime EventDateTime => EventDate.Add(KickOffTime);

        public int Rowspan
        {
            get
            {
                var rowspan = 2;

                if (Assignment.FastMarket.Any() || MatchSetting.FastMarket.IsHasMaxbetSpread)
                {
                    rowspan++;
                }

                if (Assignment.AllInOne.Any())
                {
                    rowspan++;
                }

                if (Assignment.Specific15Mins.Any() || MatchSetting.Specific15Mins.IsHasMaxbetSpread)
                {
                    rowspan++;
                }

                if (Assignment.Specific10Mins.Any() || MatchSetting.Specific10Mins.IsHasMaxbetSpread)
                {
                    rowspan++;
                }

                return rowspan;
            }
        }

        public string OtherTimeDisplay => OtherTime.HasValue ?
            OtherTime.Value.ToOWPTimeFormat().Replace(" ", String.Empty)
            : string.Empty;

        public string KickOffTimeDisplay => KickOffTime.ToOWPTimeFormat();

        public string EventDateDisplay => EventDate.ToKOTimeDateFormat();

        public string ReadyDateTimeDisplay => ReadyDateTime.ToString("hh:mm tt MMM dd, yyyy");

        public MatchInfo Clone()
        {
            var serialized = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<MatchInfo>(serialized);
        }
    }
}
