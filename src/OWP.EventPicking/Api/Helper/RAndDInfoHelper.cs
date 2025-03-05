using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api.Helper;

public static class RAndDInfoHelper
{
    public static void Build(List<EventPickingModel> matches, List<RAndDInfo> rAndDInfos)
    {
        matches.ForEach(x => x.RAndDInfo = BuildRAndDValue(x.InfoSiteMatchId, rAndDInfos));
    }

    private static List<RAndDInfo> BuildRAndDValue(int InfositeId, List<RAndDInfo> rAndDInfos)
        => rAndDInfos.Where(x => x.InfositeId == InfositeId).ToList();
}
