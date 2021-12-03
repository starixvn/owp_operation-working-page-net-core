using Newtonsoft.Json;

namespace OWP.AutoAssignment.Models.PublicApi
{
    public class RealMatchSender : CommonMatchSender
    {
        public static int[] PickedBettypes => new[] { 0 };

        [JsonProperty("Is_Futsal")]
        public bool IsFutsal { get; set; }

        public bool HasTV { get; set; }

        [JsonProperty("MaxBet_MainMatch")]
        public decimal? MaxBetMainMatch { get; set; }

        [JsonProperty("MaxBet_NoOfCorner", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MaxBetNoOfCorner { get; set; }

        [JsonProperty("MaxBet_NextCorner", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MaxBetNextCorner { get; set; }

        public bool HasReporter { get; set; }

        public bool RequireChinese { get; set; }

        public bool RequireEnglish { get; set; }

        public bool IsSoloMatch { get; set; }

        public bool IsMajorLeague { get; set; }

        public string ExtraTime { get; set; }

        public int[] Provider { get; set; }

        public static string Group => "FT";

        public bool IsFantasy { get; set; }

        public bool HasChannel { get; set; }
    }
}