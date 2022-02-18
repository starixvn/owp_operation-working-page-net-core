using Newtonsoft.Json;

namespace OWP.AutoAssignment.Models.PublicApi
{
    public class SabaMatchSender : CommonMatchSender
    {
        [JsonProperty("SABA_type")]
        public int? SabaType { get; set; }

        public decimal? MaxBet { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }
    }
}