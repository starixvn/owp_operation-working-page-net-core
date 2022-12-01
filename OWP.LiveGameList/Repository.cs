using Fanex.Data;
using OWP.LiveGameList.Criteria;
using OWP.LiveGameList.Models;

namespace OWP.LiveGameList;

public interface Repository
{
    (IEnumerable<MatchInfo>, IEnumerable<Assignment>, IEnumerable<MatchSetting>, IEnumerable<Television>, IEnumerable<MatchOffer>, IEnumerable<TicketInfo>) GetEventInformation(GetEventsCriteria criteria);
}

public class RepositoryImp : Repository
{
    public (IEnumerable<MatchInfo>, IEnumerable<Assignment>, IEnumerable<MatchSetting>, IEnumerable<Television>, IEnumerable<MatchOffer>, IEnumerable<TicketInfo>) GetEventInformation(GetEventsCriteria criteria)
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
                var tickets = reader.Read<TicketInfo>();
                return (matchInfos, assignments, matchSettings, televisions, matchOffers, tickets);
            }
        }

    }

}



