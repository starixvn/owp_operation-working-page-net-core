using Fanex.Data.Repository;
using OWP.NewLeagueSetting.Command;

namespace OWP.NewLeagueSetting.Data;

public interface Repository
{
    Task InitData(IList<LeagueSettingInfo> leagueInfo, CancellationToken cancellationToken);

    Task<IEnumerable<LeagueSettingInfoSender>> QueryLeagueSetting(LeagueParamRequest request);

    Task<LeagueSettingInfoSender> QueryLeagueSettingDetail(int leagueSettingId);

    Task UpdateLeagueSetting(UpdateLeagueSettingRequest request, int actorId);

    Task ExecuteNonQueryCommand(NonQueryCommand criteria);

    Task<IList<DbLogInfo>> QueryLog(int leagueSettingId);
}

public class RepositoryImp(IDynamicRepository repository) : Repository
{
    public async Task InitData(IList<LeagueSettingInfo> leagueInfo, CancellationToken cancellationToken)
    {
        var xml = XmlConverter.Serialize(leagueInfo);
        await repository.ExecuteAsync(new InitDataCriteria
        {
            XML = xml
        });
    }

    public async Task<IEnumerable<LeagueSettingInfoSender>> QueryLeagueSetting(LeagueParamRequest request)
    {
        var criteria = new QueryLeagueSettingCriteria
        {
            League = request.LeagueName,
            IsEqualLeague = request.IsEqualLeague
        };
        var result = await repository.FetchAsync<LeagueSettingInfoSender>(criteria);

        var filteredResult = FilterLeagueSettingsByRequest(result, request);
        var sortedResult = SortLeagueSettings(filteredResult);

        return sortedResult;
    }

    private static IEnumerable<LeagueSettingInfoSender> FilterLeagueSettingsByRequest(IEnumerable<LeagueSettingInfoSender> leagueSettings, LeagueParamRequest request)
        => leagueSettings
            .Where(x =>
                (request.Blacklist == null || request.Blacklist.Contains(x.IsBlackList ? 1 : 0))
                //&& (request.OfferLive == null || request.OfferLive.Contains(x.NoOfferLive))
                && (request.LeagueGroupId == null || request.LeagueGroupId == x.LeagueGroupId))
            .ToList();

    private static IEnumerable<LeagueSettingInfoSender> SortLeagueSettings(IEnumerable<LeagueSettingInfoSender> leagueSettings)
    {
        var leagueList = leagueSettings.ToList();

        return leagueList
            .OrderBy(x => x.LeagueGroup != "MJ")
            .ThenBy(x => NormalizeLeagueName(x.League), StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static string NormalizeLeagueName(string leagueName)
    {
        return string.IsNullOrWhiteSpace(leagueName) ? string.Empty : leagueName.TrimStart('*').Trim();
    }

    public async Task<LeagueSettingInfoSender> QueryLeagueSettingDetail(int leagueSettingId)
        => await repository.GetAsync<LeagueSettingInfoSender>(new QueryLeagueSettingDetailCriteria
        { LeagueSettingId = leagueSettingId });

    public async Task UpdateLeagueSetting(UpdateLeagueSettingRequest request, int actorId)
    {
        var criteria = new UpdateLeagueSettingCriteria
        {
            Comment = request.Comment,
            FullConditionDescription = request.FullConditionDescription,
            LeagueGroupId = request.LeagueGroupId,
            LeagueSettingId = request.LeagueSettingId,
            LstNote = request.LstNote,
            Remark2 = request.Remark2,
            MaxbetWD_HDP_Note = request.MaxbetWD_HDP_Note,
            MaxbetWD_HDP_Value = request.MaxbetWD_HDP_Value,
            WD_NoOfHDP_Note = request.WD_NoOfHDP_Note,
            WD_NoOfHDP_Value = request.WD_NoOfHDP_Value,
            MaxbetWK_HDP_Note = request.MaxbetWK_HDP_Note,
            MaxbetWK_HDP_Value = request.MaxbetWK_HDP_Value,
            WK_NoOfHDP_Note = request.WK_NoOfHDP_Note,
            WK_NoOfHDP_Value = request.WK_NoOfHDP_Value,
            OWPLeagueId = request.OWPLeagueId,
            Spread_HDP_Value = request.Spread_HDP_Value,
            Spread_Note = request.Spread_Note,
            Spread_OU_Value = request.Spread_OU_Value,
            ActorId = actorId,
            NoOfferLive = request.NoOfferLive
        };
        await repository.ExecuteAsync(criteria);
    }

    public async Task ExecuteNonQueryCommand(NonQueryCommand criteria)
        => await repository.ExecuteAsync(criteria);

    public async Task<IList<DbLogInfo>> QueryLog(int leagueSettingId)
    {
        var logs = await repository.FetchAsync<DbLogInfo>(new QueryLogCriteria
        {
            SettingId = leagueSettingId
        });
        return logs.ToList();
    }
}