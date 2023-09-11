using OWP.Event.EventShared.Models;
using OWP.Models;
using OWP.Services;

namespace OWP.Event.EventShared.Services;

public interface InfoSiteTimeServices
{
    Task<List<InfoSiteTimeModel>> GetInfositeInfo(string parentMatchIds);

    void BuildInfoSiteTime(MatchInfo matchInfo, List<InfoSiteTimeModel> infoSiteInfo);
}

public class InfoSiteTimeServicesImp : InfoSiteTimeServices
{
    private readonly GetInfoSiteTimeService getInfoSiteTimeService;

    public InfoSiteTimeServicesImp(GetInfoSiteTimeService getInfoSiteTimeService)
    {
        this.getInfoSiteTimeService = getInfoSiteTimeService;
    }

    public async Task<List<InfoSiteTimeModel>> GetInfositeInfo(string parentMatchIds)
    {
        var infoSiteInfo = await getInfoSiteTimeService.GetInfoSiteTime(parentMatchIds);

        return infoSiteInfo.ToList();
    }

    public void BuildInfoSiteTime(MatchInfo matchInfo, List<InfoSiteTimeModel> infoSiteInfo)
    {
        var infoSite = infoSiteInfo.FirstOrDefault(x => x.ParentMatchId == matchInfo.ParentMatchId);
        matchInfo.InfoSiteTime = infoSite?.KickOffTime;
        matchInfo.InfoSiteTimeDisplay = infoSite?.KickOffTimeDisplay;
    }
}