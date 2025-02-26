using OWP.EventPicking.Api.Constant;
using System.Text.Json.Serialization;

namespace OWP.EventPicking.Api.Models;
public class RAndDInfo
{
    public int InfositeId { get; set; }

    public int UpdateTypeId { get; set; }

    public string RAndDUpdate { get; set; }

    public string OldValue { get; set; }

    public string NewValue { get; set; }

    public string Group { get; set; }

    public string BgColor => Color.Item1;

    public string TextColor => Color.Item2;

    [JsonIgnore]
    public bool HomeUpdated { get; set; }

    [JsonIgnore]
    public bool AwayUpdated { get; set; }

    [JsonIgnore]
    public (string, string) Color => RAndDColorConstant.MAP_R_AND_D_COLOR.GetValueOrDefault(UpdateTypeId);
}
