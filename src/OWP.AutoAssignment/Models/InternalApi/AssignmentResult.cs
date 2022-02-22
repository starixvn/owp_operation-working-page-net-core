using Newtonsoft.Json;
using OWP.AutoAssignment.Utils.XMLExtensions;

namespace OWP.AutoAssignment.Models.InternalApi
{
    [XMLName("Row")]
    public class AssignmentResult
    {
        [XMLName("InfoSiteMatchId")]
        public int MatchId { get; set; }

        public int Bettype { get; set; }

        [JsonProperty("Is_SABA")]
        public int IsSaba { get; set; }

        [JsonProperty("Is_Futsal")]
        public int IsFutsal { get; set; }

        [JsonProperty("PC_Code")]
        public int PcCode { get; set; }

        [JsonProperty("FTHDP_TraderId")]
        public int? UserId_FTHDP { get; set; }

        [JsonProperty("OHHDP_TraderId")]
        public int? UserId_1HHDP { get; set; }

        [JsonProperty("FTOU_TraderId")]
        public int? UserId_FTOU { get; set; }

        [JsonProperty("OHOU_TraderId")]
        public int? UserId_1HOU { get; set; }

        [JsonProperty("LA_SusId")]
        public int? UserId_LA { get; set; }

        [JsonProperty("Group_LA")]
        public string Group { get; set; }

        [JsonProperty("Group_Trader")]
        public string GroupTrader { get; set; }

        public string ExtraTime { get; set; }

        public int StatusCode { get; set; }

        [JsonProperty("SABA_type")]
        public int? SabaType { get; set; }
    }
}