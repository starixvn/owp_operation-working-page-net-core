namespace OWP.AssignmentUpload.Submit.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria;
    using Fanex.Data.Repository;
    using Models;

    public interface QueryRepository
    {
        Task<AssignmentSnapshot> Query(DateTime eventDate);

        Task<List<OldMatch>> GetOldMatchIds(DateTime eventDate);
    }

    public class QueryRepositoryImp : QueryRepository
    {
        private readonly IDynamicRepository repository;

        public QueryRepositoryImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<AssignmentSnapshot> Query(DateTime eventDate)
        {
            var matchesTask = repository.FetchAsync<GeneralMatch>(new GetAllMatchesByTimeRangeCriteria { EventDate = eventDate });
            var assignmentsTask = repository.FetchAsync<MatchAssignment>(new GetAllAssignmentsByTimeRangeCriteria { EventDate = eventDate });
            var matchSettingsTask = repository.FetchAsync<MatchSetting>(new GetAllMatcheSettingsByTimeRangeCriteria { EventDate = eventDate });

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
}
