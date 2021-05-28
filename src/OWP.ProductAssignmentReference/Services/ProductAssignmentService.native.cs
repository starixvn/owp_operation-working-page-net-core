using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Caching;
using Newtonsoft.Json;
using OWP.ProductAssignmentReference.Models;

namespace OWP.ProductAssignmentReference.Services
{
    public interface ProductAssignmentService
    {
        Task<IList<UserInfo>> QueryUser(QueryUserInfo queryInfo);

        Task UpdateUserInfo(UserInfoEditor info);

        Task<IEnumerable<ReferenceResource>> GetResourceByName(string name);

        Task<LastUpdateInfo> GetLastUpdate();
    }

    public class ProductAssignmentServiceImp : ProductAssignmentService
    {
        private const string KEY_ASSIGNMENT_CONFIGURATION = "OWP-assignment-configuration";
        private const int CACHE_EXPIRED_IN_DAYS = 1;
        private readonly ICache cache;
        private readonly ProductAssignmentRepository repository;

       

        public ProductAssignmentServiceImp(
            ProductAssignmentRepository repository,
            ICache cache)
        {
            this.repository = repository;
            this.cache = cache;
        }


        public async Task<LastUpdateInfo> GetLastUpdate()
        {
            var cacheKey = KEY_ASSIGNMENT_CONFIGURATION;
            var (_, value) = await cache.TryGetAsync<string>(cacheKey);

            var assignments = string.IsNullOrEmpty(value)
                ? new LastUpdateInfo()
                : JsonConvert.DeserializeObject<LastUpdateInfo>(value);

            return assignments;
        }

        public async Task<IEnumerable<ReferenceResource>> GetResourceByName(string name)
            => await repository.GetResourceByName(name);

        public async Task<IList<UserInfo>> QueryUser(QueryUserInfo queryInfo)
            => await repository.QueryUser(queryInfo);

        public async Task UpdateUserInfo(UserInfoEditor info)
        {
            await repository.UpdateUserInfo(info);
            await SaveLastUpdate(new LastUpdateInfo
            {
                Username = info.ActorName,
                Date = DateTime.Now
            });
        }

        private async Task SaveLastUpdate(LastUpdateInfo info)
        {
            var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CACHE_EXPIRED_IN_DAYS));
            var assignmentSnapshotInJson = JsonConvert.SerializeObject(info);
            var cacheKey = KEY_ASSIGNMENT_CONFIGURATION;
            await cache.SetAsync(cacheKey, assignmentSnapshotInJson, cachePolicy);
        }
    }
}
