namespace OWP.OtherSport.EventAssignment.Services
{
    using AutoMapper;
    using Fanex.Data.Repository;
    using OWP.OtherSport.EventAssignment.Builder;
    using OWP.OtherSport.EventAssignment.Criteria;
    using OWP.OtherSport.EventAssignment.Helper;
    using OWP.OtherSport.EventAssignment.Models;
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

        private readonly AssignmentMaxbetBuilder assignmentMaxbetBuilder;

        private readonly BettypeBuilder bettypeBuilder;

        private readonly IMapper mapper;

        public CommandRepositoryImp(
            IDynamicRepository dynamicRepository,
            AssignmentMaxbetBuilder assignmentMaxbetBuilder,
            BettypeBuilder bettypeBuilder,
            IMapper mapper)
        {
            this.dynamicRepository = dynamicRepository;
            this.assignmentMaxbetBuilder = assignmentMaxbetBuilder;
            this.bettypeBuilder = bettypeBuilder;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<EventAssignmentModel>> Query(QueryEventAssignmentRequest request)
        {
            var criteria = new GetEventAssignmentCriteria
            {
                FromDate = request.From,
                ToDate = request.To,
                Sports = string.Join(",", request.Sports),
                MatchStatuses = string.Join(",", request.MatchStatuses)
            };

            var (matches, assignments, maxbets) = await dynamicRepository.FetchMultipleAsync<MatchInfo, MatchAssignment, MatchMaxBet>(criteria);

            assignments = FilterHelper.RemoveAssignmentHasNoteInvalid(assignments.ToList());

            assignments = assignments.OrderBy(x => x.Order);

            GroupTraderMaxbet(assignments.ToList(), maxbets.ToList());

            var matchsAssignment = FilterHelper.FilterMatchByEmployee(request.Employee, matches.ToList(), DicMatchAssignment);

            var matchs = mapper.Map<IList<EventAssignmentModel>>(matchsAssignment);

            BuildTraderMaxbet(matchs.ToList());

            return matchs;
        }

        public void BuildTraderMaxbet(List<EventAssignmentModel> assignments)
        {
            bettypeBuilder.BuildBettype(
                assignments,
                DicMatchAssignment);
            assignmentMaxbetBuilder.Build(assignments, DicMatchMaxBet, DicMatchAssignment);
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