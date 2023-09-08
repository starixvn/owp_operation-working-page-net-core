using OWP.EventPicking.Models;
using OWP.Extensions;

namespace OWP.EventPicking.Services;
public interface BuildRAndDInfo
{
    void Build(List<EventPickingModel> matches, List<RAndDInfo> rAndDInfos);
}
public class BuildRAndDInfoImp : BuildRAndDInfo
{
    public void Build(List<EventPickingModel> matches, List<RAndDInfo> rAndDInfos)
    {
       
        matches.ForEach(x => x.RAndDInfos = BuildRAndDValue(x.InfoSiteMatchId, rAndDInfos));
    }

    private List<RAndDInfo> BuildRAndDValue(int InfositeId, List<RAndDInfo> rAndDInfos)
        => rAndDInfos.Where(x => x.InfositeId == InfositeId).ToList();
}

