using Fanex.Data.Repository;
using Fanex.NAP.Client;
using Olympus.Xml;
using OWP.AssignmentUpload.Submit.Criteria;
using OWP.AssignmentUpload.Submit.Models;
using OWP.AssignmentUpload.Validation.Models;

namespace OWP.AssignmentUpload.Submit.Services;

public interface CommandRepository
{
    Task DeleteMatches(string deletedMatchIdsInString);

    Task InsertOrUpdateMatchAgents(List<AssignmentModel> assignments, string matchIds);

    Task InsertOrUpdateMatchAssignments(List<AssignmentModel> assignments, bool isUploadFMAndAIO, string matchIds);

    Task InsertOrUpdateMatchOffers(List<AssignmentModel> assignments, string matchIds);

    Task InsertOrUpdateMatchOtherProviders(List<AssignmentModel> assignments, string matchIds);

    Task InsertOrUpdateMatchProviders(List<AssignmentModel> assignments, string matchIds);

    Task InsertOrUpdateMatchSettings(List<AssignmentModel> assignments, bool isUploadFMAndAIO, string matchIds);

    Task InsertOrUpdateMatchTelevisions(List<AssignmentModel> assignments, string matchIds);

    Task<IEnumerable<MatchByHashKey>> InsertOrUpdateMatches(List<AssignmentModel> assignments, DateTime eventDate);

    Task Update(List<OldMatch> oldMatches, List<AssignmentModel> assignments, SubmitRequest request);
}

public class CommandRepositoryImp : CommandRepository
{
    private readonly IDynamicRepository repository;

    private readonly INapSession currentSession;

    private readonly AssignmentAnalyzer assignmentAnalyzer;

    private readonly CacheManipulator cacheManipulator;

    public CommandRepositoryImp(
        IDynamicRepository repository,
        INapSession currentSession,
        AssignmentAnalyzer assignmentAnalyzer,
        CacheManipulator cacheManipulator)
    {
        this.repository = repository;
        this.currentSession = currentSession;
        this.assignmentAnalyzer = assignmentAnalyzer;
        this.cacheManipulator = cacheManipulator;
    }

    public async Task Update(List<OldMatch> oldMatches, List<AssignmentModel> assignments, SubmitRequest request)
    {
        var hashKeyMap = (await InsertOrUpdateMatches(assignments, request.EventDate)).ToList();
        assignmentAnalyzer.ReassignMatchId(assignments, hashKeyMap);

        var newMatchIds = hashKeyMap.Select(h => h.OWPMatchId).Distinct().ToList();
        var newMatchIdsInString = string.Join(",", newMatchIds);
        var deletedMatchIds = oldMatches.Select(m => m.OWPMatchId).Except(newMatchIds);
        var deletedMatchIdsInString = string.Join(",", deletedMatchIds);

        await UpdateMatchExtraInformation(assignments, request, newMatchIdsInString, deletedMatchIdsInString);
        await SaveDeletedInfoSiteMatches(oldMatches, assignments, request.EventDate);
    }

    private async Task SaveDeletedInfoSiteMatches(List<OldMatch> oldMatches, List<AssignmentModel> assignments, DateTime kickoffDate)
    {
        var minorMatches = assignments.Where(x => !string.IsNullOrEmpty(x.ParentHashKey)).ToList();
        minorMatches
            .ForEach(x => x.InfoSiteMatchId = assignments.FirstOrDefault(y => y.HashKey == x.ParentHashKey).InfoSiteMatchId);
        var newMinorMatches = minorMatches.Select(x => new OldMatch { InfoSiteMatchId = x.InfoSiteMatchId, MinorBettypeId = x.MinorBettypeId });
        var deletedMinorMatches = oldMatches
            .Where(m => !m.IsSaba && m.MinorBettypeId.HasValue)
            .Except(newMinorMatches, new OldMatch())
            .Select(m => new MatchKickoffDate { MatchId = m.InfoSiteMatchId, KickoffDate = kickoffDate, MinorBettypeId = m.MinorBettypeId })
            .ToList();

        var newMainMatches = assignments
            .Select(a => a.InfoSiteMatchId)
            .Distinct();
        var deletedMainMatches = oldMatches
            .Select(m => m.InfoSiteMatchId)
            .Except(newMainMatches)
            .Select(m => new MatchKickoffDate { MatchId = m, KickoffDate = kickoffDate, MinorBettypeId = null })
            .ToList();

        await cacheManipulator.SaveDeletedInfoSiteMatches(deletedMainMatches.Union(deletedMinorMatches));
    }

    private async Task UpdateMatchExtraInformation(List<AssignmentModel> assignments, SubmitRequest request, string matchIdsInString, string deletedMatchIdsInString)
    {
        await Task.WhenAll(
            InsertOrUpdateMatchAgents(assignments, matchIdsInString),
            InsertOrUpdateMatchAssignments(assignments, request.IsUploadFMAndAIO, matchIdsInString),
            InsertOrUpdateMatchOffers(assignments, matchIdsInString),
            InsertOrUpdateMatchOtherProviders(assignments, matchIdsInString),
            InsertOrUpdateMatchProviders(assignments, matchIdsInString),
            InsertOrUpdateMatchSettings(assignments, request.IsUploadFMAndAIO, matchIdsInString),
            InsertOrUpdateMatchTelevisions(assignments, matchIdsInString),
            InsertOrUpdateUploadHistory(request.EventDate),
            DeleteMatches(deletedMatchIdsInString)
        );
    }

    public async Task<IEnumerable<MatchByHashKey>> InsertOrUpdateMatches(List<AssignmentModel> assignments, DateTime eventDate)
    {
        var criteria = new InsertOrUpdateMatchCriteria
        {
            Assignments = XmlConverter.Serialize(assignments),
            EventDate = eventDate,
            UserId = currentSession.UserId
        };

        return (await repository.FetchAsync<MatchByHashKey>(criteria)).ToList();
    }

    public async Task InsertOrUpdateMatchAgents(List<AssignmentModel> assignments, string matchIds)
    {
        var matchAgents = assignments.Select(a => assignmentAnalyzer.GetAgentOfMatch(a)).SelectMany(a => a);
        var command = new InsertOrUpdateMatchAgentsCommand
        {
            MatchAgents = XmlConverter.Serialize(matchAgents),
            MatchIds = matchIds
        };

        await repository.ExecuteAsync(command);
    }

    public async Task InsertOrUpdateMatchAssignments(List<AssignmentModel> assignments, bool isUploadFMAndAIO, string matchIds)
    {
        var matchAssignments = assignments.Select(a => assignmentAnalyzer.GetAssignmentOfMatch(a))
            .SelectMany(a => a);

        var command = new InsertOrUpdateMatchAssignmentsCommand
        {
            MatchAssignments = XmlConverter.Serialize(matchAssignments),
            IsUploadFMAndAIO = isUploadFMAndAIO,
            MatchIds = matchIds
        };

        await repository.ExecuteAsync(command);
    }
    public async Task InsertOrUpdateMatchOffers(List<AssignmentModel> assignments, string matchIds)
    {
        var matchOffers = assignments.Select(a => assignmentAnalyzer.GetOffersOfMatch(a)).SelectMany(a => a);
        var command = new InsertOrUpdateMatchOffersCommand
        {
            MatchOffers = XmlConverter.Serialize(matchOffers),
            MatchIds = matchIds
        };

        await repository.ExecuteAsync(command);
    }

    public async Task InsertOrUpdateMatchOtherProviders(List<AssignmentModel> assignments, string matchIds)
    {
        var matchProviders = assignments.Where(m => !string.IsNullOrEmpty(m.OtherProvider))
            .Select(a => new MatchProvider { MatchId = a.OWPMatchId, Provider = a.OtherProvider });

        var command = new InsertOrUpdateMatchOtherProvidersCommand
        {
            MatchProviders = XmlConverter.Serialize(matchProviders),
            MatchIds = matchIds
        };

        await repository.ExecuteAsync(command);
    }

    public async Task InsertOrUpdateMatchProviders(List<AssignmentModel> assignments, string matchIds)
    {
        var matchProviders = assignments
            .Where(m => !string.IsNullOrEmpty(m.Provider))
            .Select(a => new MatchProvider { MatchId = a.OWPMatchId, Provider = a.Provider });
        var command = new InsertOrUpdateMatchProvidersCommand
        {
            MatchProviders = XmlConverter.Serialize(matchProviders),
            MatchIds = matchIds
        };

        await repository.ExecuteAsync(command);
    }

    public async Task InsertOrUpdateMatchSettings(List<AssignmentModel> assignments, bool isUploadFMAndAIO, string matchIds)
    {
        var matchSettings = assignments.Select(a => assignmentAnalyzer.GetSettingsOfMatch(a))
            .SelectMany(a => a).Where(a => a.Spread != null || a.MaxBet != null);

        var command = new InsertOrUpdateMatchSettingsCommand
        {
            MatchSettings = XmlConverter.Serialize(matchSettings),
            IsUploadFMAndAIO = isUploadFMAndAIO,
            MatchIds = matchIds
        };

        await repository.ExecuteAsync(command);
    }

    public async Task InsertOrUpdateMatchTelevisions(List<AssignmentModel> assignments, string matchIds)
    {
        var matchTelevisions = assignments.Select(a => assignmentAnalyzer.GetTelevisionOfMatch(a)).SelectMany(a => a);
        var command = new InsertOrUpdateMatchTelevisionsCommand
        {
            MatchTVs = XmlConverter.Serialize(matchTelevisions),
            MatchIds = matchIds
        };

        await repository.ExecuteAsync(command);
    }

    public async Task InsertOrUpdateUploadHistory(DateTime eventDate)
    {
        var command = new InsertOrUpdateUploadHistoryCommand
        {
            EventDate = eventDate
        };

        await repository.ExecuteAsync(command);
    }

    public async Task DeleteMatches(string deletedMatchIdsInString)
    {
        var command = new DeleteMatchesCommand { MatchIds = deletedMatchIdsInString };

        await repository.ExecuteAsync(command);
    }
}