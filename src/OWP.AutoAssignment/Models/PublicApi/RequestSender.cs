namespace OWP.AutoAssignment.Models.PublicApi;

using Newtonsoft.Json;

public class RequestSender
{
    public string Action { get; set; }

    [JsonIgnore]
    public DateTime EventDate { get; set; }

    public string RequestId { get; set; }

    public DataHolder DataHolder { get; set; }
}