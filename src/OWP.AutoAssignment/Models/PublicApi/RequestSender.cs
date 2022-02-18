using Newtonsoft.Json;

namespace OWP.AutoAssignment.Models.PublicApi
{
    public class RequestSender
    {
        public string Action { get; set; }

        [JsonIgnore]
        public DateTime EventDate { get; set; }

        public string RequestId { get; set; }

        public DataHolder DataHolder { get; set; }
    }
}