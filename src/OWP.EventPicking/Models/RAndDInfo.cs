
using System.Text.Json.Serialization;
using OWP.EventPicking.Constant;

namespace OWP.EventPicking.Models;
public class RAndDInfo
{
    public int InfositeId { get; set; }

    public string UpdateType { get; set; }

    public string RAndDUpdate { get; set; }

    public string TextColor => Color.Item1;

    public string BgColor => Color.Item2;

    [JsonIgnore]
    public (string, string) Color => RAndDColorConstant.RAndDColorDict.GetValueOrDefault(UpdateType);
}
