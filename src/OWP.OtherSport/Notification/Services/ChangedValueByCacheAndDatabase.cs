
namespace OWP.OtherSport.Notification.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OWP.OtherSport.Notification.Models;
    using QueryRepository = Submit.Services.CommandRepository;

    public interface ChangedValueByCacheAndDatabase
    {
        Task<List<ChangedValue>> Create(string sessionId, DateTime eventDate);
    }

    public class ChangedValueCacheAndDatabaseImp : ChangedValueByCacheAndDatabase
    {
        private readonly CacheManipulator cacheManipulator;
        private readonly QueryRepository queryRepository;
        private readonly ChangedValueBySnapshot changedValueBySnapshot;

        public async Task<List<ChangedValue>> Create(string sessionId, string eventDate)
        {
            var oldSnapshot = await cacheManipulator.GetAssignmentSnapshot(sessionId, eventDate);
            var newSnapshot = await queryRepository.Query(eventDate);

            await cacheManipulator.DeleteAssignmentSnapshot(sessionId, eventDate);

            return changedValueBySnapshot.Create(oldSnapshot, newSnapshot);
        }
    }
}