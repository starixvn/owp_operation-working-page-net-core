using Newtonsoft.Json;

namespace OWP.AutoAssignment.Models.PublicApi
{
    public class SabaMatch : CommonMatchSender
    {
        [JsonProperty("SABA_type")]
        public int SabaType { get; set; }
    }
}