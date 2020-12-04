using Newtonsoft.Json;

namespace OWP.API.SyncMaxbet.Models
{
    public class TraderAssignmentSender
    {
        [JsonProperty("HDP")]
        public int Handicap { get; set; }

        [JsonProperty("OU")]
        public int OverUnder { get; set; }

        [JsonProperty("1H_HDP")]
        public int FirstHalfHandicap { get; set; }

        [JsonProperty("1H_HDP")]
        public int FirstHalfOverUnder { get; set; }
    }
}
