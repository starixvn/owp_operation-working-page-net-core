namespace OWP.OtherSport.EventAssignment.Services
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using OWP.OtherSport.EventAssignment.Criteria;
    using OWP.OtherSport.EventAssignment.Models;
    using OWP.OtherSport.Submit.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public interface CommandRepository
    {
        Task<List<EventAssignmentModel>> Query(QueryModel queryModel);
    }

    public class CommandRepositoryImp : CommandRepository
    {
        private readonly IDynamicRepository dynamicRepository;

        public CommandRepositoryImp (IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }
        public async Task<List<EventAssignmentModel>> Query(QueryModel queryModel)
        {
            var criteria = new GetEventAssignmentCriteria
            {
                FromDate = queryModel.FromDate,
                ToDate = queryModel.ToDate
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = await db.QueryMultipleAsync(criteria))
                {
                    var matches = (await reader.ReadAsync<MatchInfo>()).ToList();
                    var assignments = (await reader.ReadAsync<MatchAssignment>()).ToList();                 
                    var maxbets = (await reader.ReadAsync<MatchMaxBet>()).ToList();                                    

                    var events = MappingInfoToEventAssignmentModel(matches, assignments, maxbets);

                    return events;
                }
            }
        }

        private static List<EventAssignmentModel> MappingInfoToEventAssignmentModel(
          List<MatchInfo> matches,
          List<MatchAssignment> assignments,
          List<MatchMaxBet> maxbets)
        {
            var events = new List<EventAssignmentModel>();

            foreach (var match in matches)
            {
                var eventInstance = new EventAssignmentModel
                {
                    OSMatchId = match.OSMatchId,
                    MatchStatus = match.MatchStatus,
                    Sport = match.Sport,
                    DateTime = match.DateTime,
                    Home = match.Home,
                    Away = match.Away,
                    League = match.League,
                    Category = match.Category,
                    NoHDP = match.NoHDP,
                    MatchOffer = match.MatchOffer,
                    AutoLink = match.AutoLink,
                    IPTV = match.IPTV,
                    Service = match.Service,
                    WB = match.WB,
                    Provider = match.Provider,
                    MaxBets = maxbets.Where(x => x.OSMatchId == match.OSMatchId).ToList(),
                    Assignments = assignments.Where(x => x.OSMatchId == match.OSMatchId).ToList()
                };    
            
                events.Add(eventInstance);
            }

            return events;
        }
    }
}
