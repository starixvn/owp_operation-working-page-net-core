
using OWP.EventPicking.Constant;

namespace OWP.EventPicking.Models;
public class RAndDInfo
{
    public int InfositeId { get; set; }

    public string UpdateType { get; set; }

    public string RAndDUpdate { get; set; }

    public (string, string) Color  => RAndDColorConstant.RAndDColorDict.GetValueOrDefault(UpdateType);
}
