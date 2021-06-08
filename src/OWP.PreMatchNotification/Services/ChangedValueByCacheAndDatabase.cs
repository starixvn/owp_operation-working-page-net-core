namespace OWP.PreMatchNotification.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OWP.PreMatchNotification.Models;

    public interface ChangedValueByCacheAndDatabase
    {
        Task<IList<ChangedValue>> Create(string sessionId, DateTime fromDate, DateTime toDate);
    }

    public class ChangedValueByCacheAndDatabaseImp : ChangedValueByCacheAndDatabase
    {
        private readonly CacheManipulator cacheManipulator;
        private readonly Repository queryRepository;

        public ChangedValueByCacheAndDatabaseImp(
            CacheManipulator cacheManipulator,
            Repository queryRepository)
        {
            this.cacheManipulator = cacheManipulator;
            this.queryRepository = queryRepository;
        }

        public async Task<IList<ChangedValue>> Create(string sessionId, DateTime fromDate, DateTime toDate)
        {
            var oldSnapshot = await cacheManipulator.GetOldInfoSiteMatchSnapshot(sessionId);
            var newSnapshot = await queryRepository.GetNewSnapshot(fromDate, toDate);

            await cacheManipulator.DeleteAssignmentSnapshot(sessionId);

            return new List<ChangedValue>();
        }
    }
}