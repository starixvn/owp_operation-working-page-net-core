using OWP.LiveGameList.Models;
using OWP.Models;

namespace OWP.Helpers;
public static class LiveGameHelper
{
    public static IList<AdminSiteMatchInfoModel> MergeAdminSiteInfo(List<AdminSiteIdModel> adminSiteId,
        List<AdminSiteOddsModel> adminSiteInfo)
    {
        var mergedList = adminSiteId.Join(adminSiteInfo,
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
