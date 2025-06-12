using System.Text.Json.Serialization;

namespace OWP.Event.TaskList.Model;

public class CreateDiscussionResponse
{
    [JsonPropertyName("discussion")]
    public DiscussionInfo Discussion { get; set; }
}

public class DiscussionInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}