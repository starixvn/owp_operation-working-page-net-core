namespace OWP.OtherSport.Submit.Services
{
    using Fanex.Caching;
    using Newtonsoft.Json;
    using OWP.OtherSport.Submit.Models;
    using OWP.OtherSport.Validation.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface CacheManipulator
    {
        Task<List<AssignmentModel>> GetNewAssignments();

        Task SaveOldAssignmentSnapshot(AssignmentSnapshot assignmentSnapshot);
    }

    public class CacheManipulatorImp: CacheManipulator
    {
        private const string KEY_OS_ASSIGNMENT = "OWP-OS-AssignmentUpload";
        private const string KEY_OLD_OS_ASSIGNMENT_SNAPSHOT = "OWP-Old-OS-Assignment";
        private readonly ICache cache;

        public CacheManipulatorImp(ICache cache)
        {
            this.cache = cache;
        }

        public async Task<List<AssignmentModel>> GetNewAssignments()
        {
            var cacheKey = $"{KEY_OS_ASSIGNMENT}";
            var cacheResult = await cache.TryGetAsync<string>(cacheKey);

            var assignments = string.IsNullOrEmpty(cacheResult.Item2)
                ? new List<AssignmentModel>()
                : JsonConvert.DeserializeObject<List<AssignmentModel>>(cacheResult.Item2);

            return assignments ?? new List<AssignmentModel>();
        }

        public async Task SaveOldAssignmentSnapshot(AssignmentSnapshot assignmentSnapshot)
        {
            var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));
            var assignmentSnapshotInJson = JsonConvert.SerializeObject(assignmentSnapshot);
            var cacheKey = $"{KEY_OLD_OS_ASSIGNMENT_SNAPSHOT}";
            await cache.SetAsync(cacheKey, assignmentSnapshotInJson, cachePolicy);
        }
    }
}
