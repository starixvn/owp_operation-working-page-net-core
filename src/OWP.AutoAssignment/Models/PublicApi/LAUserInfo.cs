namespace OWP.AutoAssignment.Models.PublicApi;

using Newtonsoft.Json;

public class LAUserInfo
{
    [JsonProperty("SusId")]
    public int UserId { get; set; }

    [JsonProperty("Lvl")]
    public int Level { get; set; }

    public bool SpeakChinese { get; set; }

    public bool SpeakEnglish { get; set; }

    public string Location { get; set; }
}