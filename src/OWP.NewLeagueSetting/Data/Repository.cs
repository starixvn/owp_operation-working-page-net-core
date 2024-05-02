
namespace OWP.NewLeagueSetting.Data;

public interface Repository
{
    Task InitData(IList<LeagueSettingInfo> leagueInfo, CancellationToken cancellationToken);

    Task<IEnumerable<LeagueSettingInfoSender>> QueryLeagueSetting(string league);
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
}