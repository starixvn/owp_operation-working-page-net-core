namespace OWP.OtherSport.Notification.Services
{
    using OWP.OtherSport.Notification.Models;
    using QueryRepository = Submit.Services.CommandRepository;

    public interface ChangedValueByCacheAndDatabase
    {
        Task<List<ChangedValue>> Create(string sessionId, DateTime fromDate, DateTime toDate);
    }

    public class ChangedValueByCacheAndDatabaseImp : ChangedValueByCacheAndDatabase
    {
        private readonly CacheManipulator cacheManipulator;
        private readonly QueryRepository queryRepository;
        private readonly ChangedValueBySnapshot changedValueBySnapshot;

        public ChangedValueByCacheAndDatabaseImp(
            CacheManipulator cacheManipulator,
            QueryRepository queryRepository,
            ChangedValueBySnapshot changedValueBySnapshot)
        {
            this.cacheManipulator = cacheManipulator;
            this.queryRepository = queryRepository;
            this.changedValueBySnapshot = changedValueBySnapshot;
        }

        public async Task<List<ChangedValue>> Create(string sessionId, DateTime fromDate, DateTime toDate)
        {
            var oldSnapshot = await cacheManipulator.GetAssignmentSnapshot(sessionId);
            var newSnapshot = await queryRepository.Query(fromDate, toDate);

            await cacheManipulator.DeleteAssignmentSnapshot(sessionId);

            return changedValueBySnapshot.Create(oldSnapshot, newSnapshot);
        }
    }
}