using Fanex.Data.Repository;
using OWP.AssignmentUpload.Submit.Criteria;
using OWP.AssignmentUpload.Submit.Models;

namespace OWP.AssignmentUpload.Submit.Services;

public interface QueryRepository
{
    Task<AssignmentSnapshot> Query(DateTime eventDate);

    Task<List<OldMatch>> GetOldMatchIds(DateTime eventDate);
}

public class QueryRepositoryImp(IDynamicRepository repository) : QueryRepository
{
    public async Task<AssignmentSnapshot> Query(DateTime eventDate)
    {
        var matchesTask = repository.FetchAsync<GeneralMatch>(new GetAllMatchesByTimeRangeCriteria { EventDate = eventDate });
        var assignmentsTask = repository.FetchAsync<MatchAssignment>(new GetAllAssignmentsByTimeRangeCriteria { EventDate = eventDate });
        var matchSettingsTask = repository.FetchAsync<MatchSetting>(new GetAllMatchesSettingsByTimeRangeCriteria { EventDate = eventDate });

        return new AssignmentSnapshot
        {
            Matches = (await matchesTask).ToDictionary(m => m.MatchId),
            Assignments = (await assignmentsTask).GroupBy(a => a.MatchId).ToDictionary(a => a.Key, a => a.ToList()),
            MatchSettings = (await matchSettingsTask).GroupBy(a => a.MatchId).ToDictionary(a => a.Key, a => a.ToList())
        };
    }

    public async Task<List<OldMatch>> GetOldMatchIds(DateTime eventDate)
    {
        var matchesTask = await repository.FetchAsync<OldMatch>(new GetAllMatchesByDateSelCriteria { EventDate = eventDate });

        return matchesTask.ToList();
    }
}