using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using Olympus.Xml;
using OWP.OtherSport.Submit.Criteria;
using OWP.OtherSport.Submit.Models;
using OWP.OtherSport.Validation.Models;
using OWP.Services;

namespace OWP.OtherSport.Submit.Services;

public interface CommandRepository
{
    Task<AssignmentSnapshot> Query(DateTime fromDate, DateTime toDate);

    Task<List<DeletedMatch>> GetOldMatchInfo(DateTime fromDate, DateTime toDate);

    Task<List<MatchByHashKey>> InsertOrUpdateMatch(List<AssignmentModel> assignments);

    Task Update(string deletedMatchIds, List<AssignmentModel> newAssignments, string newMatchIdsInString);
}

public class CommandRepositoryImp : CommandRepository
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

    public async Task<AssignmentSnapshot> Query(DateTime fromDate, DateTime toDate)
    {
        var criteria = new GetAllMatchesByDateTimeListCriteria
        {
            FromDate = fromDate,
            ToDate = toDate
        };

        var (firstResultSet, secondResultSet, thirdResultSet) = await repository.FetchMultipleAsync<MatchInfo, MatchAssignment, MatchSetting>(criteria);

        return new AssignmentSnapshot
        {
            Matches = firstResultSet.ToDictionary(m => m.OSMatchId),
            MatchAssignments = secondResultSet.GroupBy(a => a.OSMatchId).ToDictionary(a => a.Key, a => a.ToList()),
            MatchSettings = thirdResultSet.GroupBy(a => a.OSMatchId).ToDictionary(a => a.Key, a => a.ToList())
        };
    }

    public async Task<List<DeletedMatch>> GetOldMatchInfo(DateTime fromDate, DateTime toDate)
    {
        var criteria = new GetMatchIdsByDateListCriteria
        {
            FromDate = fromDate,
            ToDate = toDate
        };

        return (await repository.FetchAsync<DeletedMatch>(criteria)).ToList();
    }

    public async Task Update(string deletedMatchIds, List<AssignmentModel> newAssignments, string newMatchIdsInString)
    {
        var assignmentMaxbets = mapper.MapBySports(newAssignments);

        await Task.WhenAll(
            InsertOrUpdateMatchAssignment(newAssignments, assignmentMaxbets, newMatchIdsInString),
            InsertOrUpdateMaxBetSpread(newAssignments, assignmentMaxbets, newMatchIdsInString),
            DeleteOldMatches(deletedMatchIds));
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