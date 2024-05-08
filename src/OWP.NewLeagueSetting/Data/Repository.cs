
using OWP.NewLeagueSetting.Query;

namespace OWP.NewLeagueSetting.Data;

public interface Repository
{
    Task InitData(IList<LeagueSettingInfo> leagueInfo, CancellationToken cancellationToken);

    Task<IEnumerable<LeagueSettingInfoSender>> QueryLeagueSetting(string league);

    Task<LeagueSettingInfoSender> QueryLeagueSettingDetail(int leagueSettingId);

    Task UpdateLeagueSetting(UpdateLeagueSettingRequest request);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository repository;

    public RepositoryImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task InitData(IList<LeagueSettingInfo> leagueInfo, CancellationToken cancellationToken)
    {
        var xml = XmlConverter.Serialize(leagueInfo);
        await repository.ExecuteAsync(new InitDataCriteria
        {
            XML = xml
        }, cancellationToken);
    }

    public async Task<IEnumerable<LeagueSettingInfoSender>> QueryLeagueSetting(string league)
        => await repository.FetchAsync<LeagueSettingInfoSender>(new QueryLeagueSettingCriteria { League = league });

    public async Task<LeagueSettingInfoSender> QueryLeagueSettingDetail(int leagueSettingId)
        => await repository.GetAsync<LeagueSettingInfoSender>(new QueryLeagueSettingDetailCriteria { LeagueSettingId = leagueSettingId });

    public async Task UpdateLeagueSetting(UpdateLeagueSettingRequest request)
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
            MaxbetWD_HDP_value = request.MaxbetWD_HDP_Value,
            MaxbetWD_NoOfHDP_Note = request.WD_NoOfHDP_Note,
            MaxbetWD_NoOfHDP_value = request.WD_NoOfHDP_Value,
            LeagueGroup = request.LeagueGroup,
            MaxbetWK_HDP_Note = request.MaxbetWK_HDP_Note,
            MaxbetWK_HDP_value = request.MaxbetWK_HDP_Value,
            MaxbetWK_NoOfHDP_Note = request.WK_NoOfHDP_Note,
            MaxbetWK_NoOfHDP_value = request.WK_NoOfHDP_Value,
            OWPLeagueId = request.OWPLeagueId,
            Spread_HDP_value = request.Spread_HDP_Value,
            Spread_Note = request.Spread_Note,
            Spread_OU_value = request.Spread_OU_Value
        };
        await repository.ExecuteAsync(criteria);
    }
}