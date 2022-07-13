namespace OWP.LeagueManagement.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class LeagueSettingCondition
    {
        [JsonProperty("LeagueSettingId")]
        public int LeagueSettingId { get; set; }
        [JsonProperty("HasTV")]
        public bool HasTV { get; set; }
        [JsonProperty("TV")]
        public string TV { get; set; } = string.Empty;
        [JsonProperty("Rounds")]
        public IEnumerable<Condition> Rounds { get; set; } = new List<Condition>();
        [JsonProperty("Divisions")]
        public IEnumerable<Condition> Divisions { get; set; } = new List<Condition>();
    }
}