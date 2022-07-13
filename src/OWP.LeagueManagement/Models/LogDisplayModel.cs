namespace OWP.LeagueManagement.Models
{
    using Newtonsoft.Json;
    public class LogDisplayModel
    {
        [JsonProperty("Product")]
        public string Product { get; set; }
        [JsonProperty("Action")]
        public string Action { get; set; }
        [JsonProperty("OldValue")]
        public string OldValue { get; set; }
        [JsonProperty("NewValue")]
        public string NewValue { get; set; }
        [JsonProperty("ModifyDate")]
        public string ModifyDate { get; set; }
        [JsonProperty("ModifyBy")]
        public string ModifyBy { get; set; }
    }
}