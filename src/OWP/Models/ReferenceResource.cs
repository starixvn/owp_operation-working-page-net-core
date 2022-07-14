namespace OWP.Models
{
    using Newtonsoft.Json;
    public class ReferenceResource
    {
        [JsonProperty("ResourceId")]
        public int ResourceId { get; set; }
        [JsonProperty("Value")]
        public string Value { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}