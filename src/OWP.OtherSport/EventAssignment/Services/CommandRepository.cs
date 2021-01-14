namespace OWP.OtherSport.EventAssignment.Services
{
    using Fanex.Data.Repository;
    using OWP.OtherSport.EventAssignment.Criteria;
    using OWP.OtherSport.EventAssignment.Models;
    using OWP.OtherSport.Submit.Contants;
    using OWP.OtherSport.Submit.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public interface CommandRepository
    {
        Task<IEnumerable<EventAssignmentModel>> Query(QueryEventAssignmentRequest request);
    }

    public class CommandRepositoryImp : CommandRepository
    {
        private readonly IDynamicRepository dynamicRepository;

        public CommandRepositoryImp(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<EventAssignmentModel>> Query(QueryEventAssignmentRequest request)
        {
            var criteria = new GetEventAssignmentCriteria
            {
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };

            var (matches, assignments, maxbets) = await dynamicRepository.FetchMultipleAsync<MatchInfo, MatchAssignment, MatchMaxBet>(criteria);          

            var events = MappingInfoToEventAssignmentModel(matches, assignments, maxbets);

            return events;
        }

        private static IEnumerable<EventAssignmentModel> MappingInfoToEventAssignmentModel(
          IEnumerable<MatchInfo> matches,
          IEnumerable<MatchAssignment> assignments,
          IEnumerable<MatchMaxBet> maxbets)
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
                    OtherProvider = match.OtherProvider,
                    WB = match.WB,
                    Provider = match.Provider,
                    Remark = match.Remark,
                    TraderMaxbetModels = GetTraderMaxbet(match.OSMatchId, assignments, maxbets)                    
                };    
            
                events.Add(eventInstance);
            }

            return events;
        }

        public static IDictionary<string, List<TraderMatBex>> GetTraderMaxbet(int osMatchId, IEnumerable<MatchAssignment> assignments,
          IEnumerable<MatchMaxBet> maxbets)
        {
            var maxbetList = maxbets.Where(x => x.OSMatchId == osMatchId).ToList();
            var assignmentList = assignments.Where(x => x.OSMatchId == osMatchId).ToList(); 
            var allList = assignmentList.Cast<TraderMatBex>().Union(maxbetList);
            var groups = allList.GroupBy(a => a.BetTypeName).ToDictionary(g => g.Key, g => g.ToList());        

            return groups;
        }
    }
}
