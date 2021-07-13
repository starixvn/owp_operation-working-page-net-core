namespace OWP.OtherSport.Submit.Services
{
    using Fanex.Data.Repository;
    using Fanex.NAP.Client.Helpers.Web;
    using Olympus.Xml;
    using OWP.OtherSport.Submit.Criteria;
    using OWP.OtherSport.Submit.Models;
    using OWP.OtherSport.Validation.Models;
    using OWP.Services;
    using System;
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

        private readonly INapSession currentSession;

        private readonly LogServiceWrapper logService;

        public CommandRepositoryImp(
            IDynamicRepository repository,
            AssignmentMaxbetMapper mapper,
            AssignmentAnalyzer assignmentAnalyzer,
            INapSession currentSession,
            LogServiceWrapper logService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.assignmentAnalyzer = assignmentAnalyzer;
            this.currentSession = currentSession;
            this.logService = logService;
        }

        public async Task<AssignmentSnapshot> Query(string eventDates)
        {
            var criteria = new GetAllMatchesByDateTimeListCriteria
            {
                EventDates = eventDates
            };

            var (firstResultSet, secondResultSet, thirdResultSet) = await repository.FetchMultipleAsync<MatchInfo, MatchAssignment, MatchMaxBet>(criteria);

            return new AssignmentSnapshot
            {
                Matches = firstResultSet.ToDictionary(m => m.OSMatchId),
                MatchAssignments = secondResultSet.GroupBy(a => a.OSMatchId).ToDictionary(a => a.Key, a => a.ToList()),
                MaxBet = thirdResultSet.GroupBy(a => a.OSMatchId).ToDictionary(a => a.Key, a => a.ToList())
            };
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
            await logService.Log("OS-Update-Input parameters", oldMatchIds, newAssignments);
            var hashKeyMap = await InsertOrUpdateMatch(newAssignments);
            var newMatchIds = hashKeyMap.Select(h => h.OSMatchId).Distinct().ToList();
            var newMatchIdsInString = string.Join(',', newMatchIds);

            var deletedMatchIdsInString = assignmentAnalyzer.DetectOldMatchIds(oldMatchIds, newMatchIds);
            assignmentAnalyzer.ReassignOSMatchId(hashKeyMap, newAssignments);
            var assignmentMaxbets = mapper.MapBySports(newAssignments);
            await logService.Log("OS-UpdateMatchExtraInformation-Input parameters", hashKeyMap, newMatchIdsInString, deletedMatchIdsInString, newAssignments);

            await Task.WhenAll(
                InsertOrUpdateMatchAssignment(newAssignments, assignmentMaxbets, newMatchIdsInString),
                InsertOrUpdateMaxBetSpread(newAssignments, assignmentMaxbets, newMatchIdsInString),
                DeleteOldMatches(deletedMatchIdsInString));
        }

        public async Task<List<MatchByHashKey>> InsertOrUpdateMatch(List<AssignmentModel> assignments)
        {
            var command = new InsertOrUpdateMatchesCommand
            {
                Matches = XmlConverter.Serialize(assignments),
                UserId = currentSession.UserId
            };

            return (await repository.FetchAsync<MatchByHashKey>(command)).ToList();
        }

        public async Task InsertOrUpdateMatchAssignment(List<AssignmentModel> assignments, Dictionary<int, List<AssignmentMaxbet>> assignmentMaxbets, string newMatchIdsInString)
        {
            var matchAssignment = assignments.Select(a => assignmentAnalyzer.GetAllAssignmentsOfMatch(a, assignmentMaxbets[a.OSMatchId])).SelectMany(a => a);
            var command = new InsertOrUpdateMatchAssignmentsCommand
            {
                MatchAssignments = XmlConverter.Serialize(matchAssignment),
                MatchIds = newMatchIdsInString
            };

            await repository.ExecuteAsync(command);
        }

        public async Task InsertOrUpdateMaxBetSpread(List<AssignmentModel> assignments, Dictionary<int, List<AssignmentMaxbet>> assignmentMaxbets, string newMatchIdsInString)
        {
            var maxBetSpreads = assignments.Select(a => assignmentAnalyzer.GetMaxBets(a, assignmentMaxbets[a.OSMatchId])).SelectMany(a => a);
            var command = new InsertOrUpdateMaxBetCommand
            {
                MaxBets = XmlConverter.Serialize(maxBetSpreads),
                MatchIds = newMatchIdsInString
            };

            await repository.ExecuteAsync(command);
        }

        public async Task DeleteOldMatches(string deletedMatchIdsInString)
        {
            var command = new DeleteOldMatchesCommand { MatchIds = deletedMatchIdsInString };

            await repository.ExecuteAsync(command);
        }
    }
}
