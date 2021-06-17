namespace OWP.PreMatchNotification.LinkedMatch.Services
{
    using Models;
    using OWP.PreMatchNotification.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface CacheManipulator
    {
        Task<List<GeneralMatch>> GetOldLGAMatchSnapshot(string sessionId, DateTime eventDate);

        Task DeleteAssignmentSnapshot(string sessionId, DateTime eventDate);
    }

    public class CacheManipulatorImp : CacheManipulator
    {
        private readonly CacheManipulatorCommon cacheManipulator;
        private const string OLD_LGAMATCHES_SNAPSHOT_KEY = "OWP-Old-LGAMatches";

        public CacheManipulatorImp(CacheManipulatorCommon cacheManipulator)
        {
            this.cacheManipulator = cacheManipulator;
        }

        public async Task<List<GeneralMatch>> GetOldLGAMatchSnapshot(string sessionId, DateTime eventDate)
        {
            var cacheKey = $"{OLD_LGAMATCHES_SNAPSHOT_KEY}-{sessionId}-{eventDate}";
            var assignmentSnapshot = await cacheManipulator.GetSingleData(cacheKey, new AssignmentSnapshot());

            return assignmentSnapshot.Matches.Values.ToList();
        }

        public async Task DeleteAssignmentSnapshot(string sessionId, DateTime eventDate)
        {
            var cacheKey = $"{OLD_LGAMATCHES_SNAPSHOT_KEY}-{sessionId}-{eventDate}";
            await cacheManipulator.RemoveData(cacheKey);
        }
    }
}
