using OWP.Models;

namespace OWP.UserManagement.Models;

public class LevelOption
{
    public IEnumerable<ReferenceResource> LevelOfLiveTrading { get; set; }

    public IEnumerable<ReferenceResource> LevelOfLiveAssistant { get; set; }

    public IEnumerable<ReferenceResource> LevelOfOtherSection { get; set; }

}