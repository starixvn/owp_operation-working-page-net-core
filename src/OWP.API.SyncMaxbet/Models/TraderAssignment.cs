using Newtonsoft.Json;

namespace OWP.API.SyncMaxbet.Models
{
    public class TraderAssignment
    {
        [JsonProperty("HDP")]
        public int Handicap { get; set; }

        [JsonProperty("OU")]
        public int OverUnder { get; set; }

        [JsonProperty("1H_HDP")]
        public int FirstHandicap { get; set; }

        [JsonProperty("1H_HDP")]
        public int FirstOverUnder { get; set; }
    }
}
