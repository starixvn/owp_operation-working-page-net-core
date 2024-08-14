using Newtonsoft.Json;

namespace OWP.TicketCenter.ViewModels;
public record ConsolidationSubtypeViewModel
{
    public int Id { get; set; }

    public  string Label { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<ConsolidationSubtypeViewModel> ChildNode { get; set; }
}
