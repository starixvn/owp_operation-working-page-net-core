using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Models;
using OWP.PreMatchNotification.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.PreMatchNotification.Assignment.Services
{
    public interface ChangedValueByCacheAndDatabase
    {
        Task<List<ChangedValue>> Create(string redisKey);
    }

    public class ChangedValueByCacheAndDatabaseImp : ChangedValueByCacheAndDatabase
    {
        private readonly CacheManipulatorCommon cacheManipulator;
        private readonly QueryRepository queryRepository;
        private readonly ChangedValueBySnapshot changedValueBySnapshot;

        public ChangedValueByCacheAndDatabaseImp(
            CacheManipulatorCommon cacheManipulator,
            QueryRepository queryRepository,
            ChangedValueBySnapshot changedValueBySnapshot)
        {
            this.cacheManipulator = cacheManipulator;
            this.queryRepository = queryRepository;
            this.changedValueBySnapshot = changedValueBySnapshot;
        }

        public async Task<List<ChangedValue>> Create(string redisKey)
        {
            var oldSnapshot = await cacheManipulator.GetData<AssignmentSnapshot>(redisKey);
            var newSnapshot = await queryRepository.Query(eventDate);
        }
    }
}
