using Newtonsoft.Json;

namespace OWP.AutoAssignment.Models.PublicApi
{
    public class SabaMatchSender : CommonMatchSender
    {
        [JsonProperty("SABA_type")]
        public int? SabaType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MaxBet { get; set; }
    }
}
