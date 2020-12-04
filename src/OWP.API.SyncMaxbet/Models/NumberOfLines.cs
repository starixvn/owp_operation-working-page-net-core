using Newtonsoft.Json;

namespace OWP.API.SyncMaxbet.Models
{
    public class NumberOfLines
    {
        [JsonProperty("credit_ft")]
        public int CreditFt { get; set; }

        [JsonProperty("credit_1h")]
        public int CreditFirstHaft { get; set; }

        [JsonProperty("licensee_ft")]
        public int LicenseeFt { get; set; }

        [JsonProperty("licensee_1h")]
        public int LicenseeFirstHaft { get; set; }
    }
}
