using OWP.Constants;
using OWP.Models;

namespace OWP.Helpers;
public static class LiveGameHelper
{
    public static IList<AdminSiteMatchInfoModel> MergeAdminSiteInfo(List<AdminSiteIdModel> adminSiteId,
        List<AdminSiteOddsModel> adminSiteInfo)
    {
        var adminSiteInfoOdds = adminSiteInfo.GroupBy(x => x.AdminSiteMatchId)
            .Where(g => g.All(x => x.OddsStatus != EventStatus.Running))
            .Select(g => g.First(x => x.OddsStatus != EventStatus.Running)).ToList();
        var mergedList = adminSiteId.Join(adminSiteInfoOdds,
                l1 => l1.AdminSiteMatchId,
                l2 => l2.AdminSiteMatchId,
                (l1, l2) => new { l1, l2 })
            .Select(x => new AdminSiteMatchInfoModel
            {
                AdminSiteMatchId = x.l1.AdminSiteMatchId,
                ParentMatchId = x.l1.ParentMatchId
            }).ToList();
        return mergedList;
    }
}
