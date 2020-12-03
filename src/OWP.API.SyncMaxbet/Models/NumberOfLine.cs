using Newtonsoft.Json;

namespace OWP.API.SyncMaxbet.model
{
    public class NumberOfLine
    {
        [JsonProperty("credit_ft")]
        public int CreditFt { get; set; }

        [JsonProperty("credit_1h")]
        public int CreditFirstH { get; set; }

        [JsonProperty("licensee_ft")]
        public int LicenseeFt { get; set; }

        [JsonProperty("licensee_ft")]
        public int LicenseeFirstH { get; set; }
    }
}
