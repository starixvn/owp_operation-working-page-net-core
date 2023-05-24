using OWP.Event.EventShared.Const;
using OWP.Event.EventShared.Models;
using OWP.Models;
using OWP.Services;

namespace OWP.Event.EventShared.Services;

public interface InfoSiteTimeServices
{
    Task<List<InfoSiteTimeModel>> GetInfositeInfo(string parentMatchIds);

    void BuildInfoSiteTimeAndAbsenceIcon(
        MatchInfo matchInfo,
        List<InfoSiteTimeModel> infoSiteInfo,
        List<AdminSiteMatchInfoModel> adminSiteMatchInfo);
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

    public void BuildInfoSiteTimeAndAbsenceIcon(MatchInfo matchInfo, List<InfoSiteTimeModel> infoSiteInfo, List<AdminSiteMatchInfoModel> adminSiteMatchInfo)
    {
        var infoSite = infoSiteInfo.FirstOrDefault(x => x.ParentMatchId == matchInfo.ParentMatchId);
        var isShowAbsenceIcon = adminSiteMatchInfo.Any(x => x.ParentMatchId == matchInfo.OWPMatchId);
        matchInfo.InfoSiteTime = infoSite?.KickOffTime;
        matchInfo.InfoSiteTimeDisplay = infoSite?.KickOffTimeDisplay;
        matchInfo.IsShowAbsenceIcon = !EventConst.StatusNotOffered.Contains(matchInfo.MatchStatus) && isShowAbsenceIcon;
    }
}