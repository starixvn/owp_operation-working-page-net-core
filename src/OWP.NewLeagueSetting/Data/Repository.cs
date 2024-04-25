using Olympus.Xml;
using OWP.NewLeagueSetting.Criteria;

namespace OWP.NewLeagueSetting.Data;

public interface Repository
{
    Task InitData(IList<LeagueSettingInfo> leagueInfo);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository repository;

    public RepositoryImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task InitData(IList<LeagueSettingInfo> leagueInfo)
    {
        var xml = XmlConverter.Serialize(leagueInfo);
        await repository.ExecuteAsync(new InitDataCriteria
        {
            XML = xml
        });
    }
}