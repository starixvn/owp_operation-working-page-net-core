namespace OWP.OtherSport.Submit.Services
{
    using Fanex.Data.Repository;
    using Fanex.NAP.Client.Helpers.Web;
    using OWP.OtherSport.Submit.Criteria;
    using OWP.OtherSport.Submit.Extensions;
    using OWP.OtherSport.Submit.Models;
    using OWP.OtherSport.Validation.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface CommandRepository
    {
        Task<AssignmentSnapshot> Query(string eventDates);

        Task<List<int>> GetOldMatchIds(string eventDateInString);

        Task Update(string eventDateInString, List<AssignmentModel> newAssignments);
    }

    public class CommandRepositoryImp: CommandRepository
    {
        private readonly IDynamicRepository repository;

        private readonly AssignmentMaxbetMapper mapper;

        private readonly AssignmentAnalyzer assignmentAnalyzer;

        private readonly INapSession session;

        public CommandRepositoryImp(
            IDynamicRepository repository,
            AssignmentMaxbetMapper mapper,
            AssignmentAnalyzer assignmentAnalyzer,
            INapSession session)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.assignmentAnalyzer = assignmentAnalyzer;
            this.session = session;
        }

        public async Task<AssignmentSnapshot> Query(string eventDates)
        {
            return new AssignmentSnapshot();
        }

        public async Task<List<int>> GetOldMatchIds(string eventDateInString)
        {
            var criteria = new GetMatchIdsByDateListCriteria
            {
                EventDates = eventDateInString
            };

            return (await repository.FetchAsync<int>(criteria)).ToList();
        }

        public async Task Update(string eventDateInString, List<AssignmentModel> newAssignments)
        {
            var oldMatchIds = await GetOldMatchIds(eventDateInString);
            var hashKeyMap = await InsertOrUpdateMatch(newAssignments);
            var newMatchIds = hashKeyMap.Select(h => h.OSMatchId).Distinct().ToList();
            var newMatchIdsInString = string.Join(',', newMatchIds);

            var deletedMatchIdsInString = assignmentAnalyzer.DetectOldMatchIds(oldMatchIds, newMatchIds);
            var assignmentMaxbets = mapper.MapBySport(newAssignments);

            await Task.WhenAll(
                InsertOrUpdateMatchAssignment(newAssignments, assignmentMaxbets, newMatchIdsInString),
                InsertOrUpdateMaxBet(newAssignments, assignmentMaxbets, newMatchIdsInString),
                DeleteOldMatches(deletedMatchIdsInString));
        }

        public async Task<List<MatchByHashKey>> InsertOrUpdateMatch(List<AssignmentModel> assignments)
        {
            var criteria = new InsertOrUpdateMatchesCommand
            {
                Matches = XMLExtensions.Serialize(assignments),
                UserId = session.UserId
            };

            return (await repository.FetchAsync<MatchByHashKey>(criteria)).ToList();
        }

        public async Task InsertOrUpdateMatchAssignment(List<AssignmentModel> assignments, Dictionary<int, List<AssignmentMaxbet>> assignmentMaxbets, string newMatchIdsInString)
        {
            var matchAssignment = assignments.Select(a => assignmentAnalyzer.GetAllAssignmentsOfMatch(a, assignmentMaxbets[a.OSMatchId]));
            var criteria = new InsertOrUpdateMatchAssignmentsCommand
            {
                MatchAssignments = XMLExtensions.Serialize(matchAssignment),
                MatchIds = newMatchIdsInString
            };

            await repository.ExecuteAsync(criteria);
        }

        public async Task InsertOrUpdateMaxBet(List<AssignmentModel> assignments, Dictionary<int, List<AssignmentMaxbet>> assignmentMaxbets, string newMatchIdsInString)
        {
            var matchAssignment = assignments.Select(a => assignmentAnalyzer.GetMaxBets(a, assignmentMaxbets[a.OSMatchId]));
            var criteria = new InsertOrUpdateMatchAssignmentsCommand
            {
                MatchAssignments = XMLExtensions.Serialize(matchAssignment),
                MatchIds = newMatchIdsInString
            };

            await repository.ExecuteAsync(criteria);
        }

        public async Task DeleteOldMatches(string deletedMatchIdsInString)
        {
            var command = new DeleteOldMatchesCommand { MatchIds = deletedMatchIdsInString };

            await repository.ExecuteAsync(command);
        }
    }
}
