using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using OWP.TaskList.Criteria;
using OWP.TaskList.Models;

namespace OWP.TaskList;

public interface Repository
{
    public (IEnumerable<MatchInfo>, IEnumerable<Assignment>, IEnumerable<MatchSetting>, IEnumerable<Television>,
        IEnumerable<MatchOffer>, IEnumerable<MainMatchInfo>, IEnumerable<TicketInfo>) GetTaskList(
            GetTaskListCriteria criteria);

    public Task<DateTime> UpdateTradeStatus(int matchId);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository dynamicRepository;

    private readonly INapSession session;

    public RepositoryImp(IDynamicRepository dynamicRepository, INapSession session)
    {
        this.dynamicRepository = dynamicRepository;
        this.session = session;
    }

    public (IEnumerable<MatchInfo>, IEnumerable<Assignment>, IEnumerable<MatchSetting>, IEnumerable<Television>,
        IEnumerable<MatchOffer>, IEnumerable<MainMatchInfo>, IEnumerable<TicketInfo>) GetTaskList(
            GetTaskListCriteria criteria)
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
                var tickets = reader.Read<TicketInfo>();
                return (matchInfos, assignments, matchSettings, televisions, matchOffers, mainMatchInfo, tickets);
            }
        }
    }

    public async Task<DateTime> UpdateTradeStatus(int matchId)
    {
        var criteria = new UpdateReadyTradeStatusCriteria
        {
            OWPMatchId = matchId,
            UserId = session.UserId
        };
        return (await dynamicRepository.FetchAsync<DateTime>(criteria)).FirstOrDefault();
    }
}