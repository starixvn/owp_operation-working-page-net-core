namespace OWP.OtherSport.EventAssignment.Services
{
    using Fanex.Data.Repository;
    using OWP.OtherSport.EventAssignment.Criteria;
    using OWP.OtherSport.EventAssignment.Models;
    using OWP.OtherSport.Helper;
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
        private Dictionary<int, List<MatchAssignment>> DicMatchAssignment;

        private Dictionary<int, List<MatchMaxBet>> DicMatchMaxBet;

        private readonly IDynamicRepository dynamicRepository;

        public CommandRepositoryImp(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<EventAssignmentModel>> Query(QueryEventAssignmentRequest request)
        {
            var criteria = new GetEventAssignmentCriteria
            {
                FromDate = request.From,
                ToDate = request.To,
                Employee = request.Employee,
                Sports = string.Join(",", request.Sports),
                MatchStatuses = string.Join(",", request.MatchStatuses)
            };

            var (matches, assignments, maxbets) = await dynamicRepository.FetchMultipleAsync<MatchInfo, MatchAssignment, MatchMaxBet>(criteria);

            GroupTraderMaxbet(assignments.ToList(), maxbets.ToList());

            var matchs = MappingInfoToEventAssignmentModelIgnoreTraderMaxbet(matches.ToList());

            BuildTraderMaxbet(matchs.ToList());

            return matchs;
        }

        public void BuildTraderMaxbet(List<EventAssignmentModel> matchs)
        {
            EventAssignmentInitBettype.InitBettype(
                matchs,
                DicMatchAssignment,
                DicMatchMaxBet);
            EventAssignmentBuildMaxbet.BuildMaxbet(matchs, DicMatchMaxBet);
            EventAssignmentBuildAssignment.BuildAssignment(matchs, DicMatchAssignment);
        }

        private IEnumerable<EventAssignmentModel> MappingInfoToEventAssignmentModelIgnoreTraderMaxbet(
          IList<MatchInfo> matches)
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
                };

                events.Add(eventInstance);
            }

            return events;
        }

        public void GroupTraderMaxbet(
             IList<MatchAssignment> assignments,
          IList<MatchMaxBet> maxbets)
        {
            DicMatchAssignment = assignments.GroupBy(assignment => assignment.OSMatchId)
               .ToDictionary(group => group.Key, group => group.ToList());

            DicMatchMaxBet = maxbets.GroupBy(maxbet => maxbet.OSMatchId)
                .ToDictionary(group => group.Key, group => group.ToList());
        }
    }
}
