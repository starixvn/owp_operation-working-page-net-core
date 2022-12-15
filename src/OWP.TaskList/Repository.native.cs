namespace OWP.TaskList;
using Fanex.Data;
using Criteria;
using Fanex.Data.Repository;
using Models;

public interface Repository
{
    public (IEnumerable<MatchInfo>, IEnumerable<Assignment>, IEnumerable<MatchSetting>, IEnumerable<Television>, IEnumerable<MatchOffer>, IEnumerable<MainMatchInfo>) GetTaskList(GetTaskListCriteria criteria);

}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository dynamicRepository;

    public RepositoryImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public (IEnumerable<MatchInfo>, IEnumerable<Assignment>, IEnumerable<MatchSetting>, IEnumerable<Television>, IEnumerable<MatchOffer>, IEnumerable<MainMatchInfo>) GetTaskList(GetTaskListCriteria criteria)
    {
        using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
        {
            using (var reader = db.QueryMultiple(criteria))
            {
                var matchInfos = reader.Read<MatchInfo>();
                var assignments = reader.Read<Assignment>();
                var matchSettings = reader.Read<MatchSetting>();
                var televisions = reader.Read<Television>();
                var matchOffers = reader.Read<MatchOffer>();
                var mainMatchInfo = reader.Read<MainMatchInfo>();
                // var tickets = reader.Read<TicketInfo>();
                return (matchInfos, assignments, matchSettings, televisions, matchOffers, mainMatchInfo);
            }
        }
    }
}