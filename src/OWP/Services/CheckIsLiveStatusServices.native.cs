using Fanex.Data.Repository;
using Olympus.OWP.Constants;
using OWP.Criteria;
using OWP.Models;
using OWP.Utils;

namespace OWP.Services;

public interface CheckIsLiveStatusServices
{
    Task<List<IsLiveMatchInfo>> GetOfferLiveStatusFromMainDB(string adminSiteIds);

    bool GetOfferLiveStatus(int? minorBettypeId, int parentAdminSiteId, List<IsLiveMatchInfo> mainDBResult);
}

public class CheckIsLiveStatusServicesImp : CheckIsLiveStatusServices
{
    private readonly IDynamicRepository dynamicRepository;

    public CheckIsLiveStatusServicesImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public bool GetOfferLiveStatus(int? minorBettypeId, int parentAdminSiteId, List<IsLiveMatchInfo> mainDBResult)
    {
        var matchByAdminSiteSubtypes = minorBettypeId == null
            ? GetOfferLiveMainMatch(parentAdminSiteId, mainDBResult)
            : GetOfferLiveMinorMatch(minorBettypeId, parentAdminSiteId, mainDBResult);

        return GetIsLiveStatus(matchByAdminSiteSubtypes.ToList());
    }

    private static IEnumerable<IsLiveMatchInfo> GetOfferLiveMinorMatch(int? minorBettypeId, int parentAdminSiteId, IEnumerable<IsLiveMatchInfo> mainDBResult)
    {
        var subType = GetSubtypes(minorBettypeId);
        if (subType == 0)
        {
            return new List<IsLiveMatchInfo>();
        }

        var (fromIndex, toIndex) = OneManyMapping.GetRangeOneManyMatch(minorBettypeId);

        return mainDBResult.Where(x =>
            x.ParentId == parentAdminSiteId &&
            x.SubType >= subType + fromIndex &&
            x.SubType <= subType + toIndex);
    }

    private static IEnumerable<IsLiveMatchInfo> GetOfferLiveMainMatch(int parentAdminSiteId, IEnumerable<IsLiveMatchInfo> mainDBResult)
        => mainDBResult.Where(x => x.MatchId == parentAdminSiteId);

    public static bool GetIsLiveStatus(List<IsLiveMatchInfo> matches)
        => matches.Any() && matches.All(x => x.OfferLive);

    public static int GetSubtypes(int? minorBettypeId)
        => MinorBettype.GetRepresentativeSubtype(minorBettypeId ?? 0);

    public async Task<List<IsLiveMatchInfo>> GetOfferLiveStatusFromMainDB(string adminSiteIds)
    {
        var criteria = new GetOfferLiveFromMainDBCriteria
        {
            AdminSiteIds = adminSiteIds
        };

        return (await dynamicRepository.FetchAsync<IsLiveMatchInfo>(criteria)).ToList();
    }
}