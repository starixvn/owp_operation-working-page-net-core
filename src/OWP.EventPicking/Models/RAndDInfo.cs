
using OWP.EventPicking.Constant;
using System.Text.Json.Serialization;

namespace OWP.EventPicking.Models;
public class RAndDInfo
{
    public int InfositeId { get; set; }

    public string UpdateType { get; set; }

    public int UpdateTypeId { get; set; }

    public string RAndDUpdate { get; set; }

    public string BgColor => Color.Item1;

    public string TextColor => Color.Item2;

    [JsonIgnore]
    public (string, string) Color => RAndDColorConstant.RAndDColorDict.GetValueOrDefault(UpdateType);
}
