
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
        });
    }

    public async Task<IEnumerable<LeagueSettingInfoSender>> QueryLeagueSetting(LeagueParamRequest request)
    { 
        var result = await repository.FetchAsync<LeagueSettingInfoSender>(new QueryLeagueSettingCriteria
        { League = request.LeagueName, IsEqualLeague = request.IsEqualLeague });
        return result;
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
}