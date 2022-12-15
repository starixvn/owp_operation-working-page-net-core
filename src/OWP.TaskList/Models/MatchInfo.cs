using OWP.Utils;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;

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

        public int TotalNotes { get; set; }

        public bool IsHasNewNote { get; set; }

        public AssignmentModel Assignment { get; set; }

        public MatchSettingModel MatchSetting { get; set; }

        [JsonIgnore]
        public IEnumerable<Television> Televisions { get; set; }

        public IEnumerable<MatchOffer> MatchOffers { get; set; }

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

        public string TimeToHtml
        {
            get
            {
                var time = new StringBuilder();
                var kickOffTime = KickOffTime.ToOWPTimeFormat();

                if (!IsTodayEvent)
                {
                    time.AppendFormat(CultureInfo.InvariantCulture, "<span class='badge-label cancelled'>{0}</span>", EventDate.ToKOTimeDateFormat());
                }

                time.AppendFormat(CultureInfo.InvariantCulture, "<p>{0}</p>", kickOffTime.Replace(" ", String.Empty));

                if (OtherTime.HasValue)
                {
                    var otherTime = OtherTime.Value.ToOWPTimeFormat();
                    time.AppendFormat(CultureInfo.InvariantCulture, "<p><span class='badge-label other-time'>{0}</span></p>", otherTime.Replace(" ", String.Empty));
                }

                return time.ToString();
            }
        }

        public string TelevisionToHtml
        {
            get
            {
                var television = string.Join(
                    ", ",
                    Televisions.Select(x => string.Format(CultureInfo.InvariantCulture, "<span style='color: {0}'>{1}</span>", x.Color, x.TVCode.Trim())));

                return television;
            }
        }
    }
}
