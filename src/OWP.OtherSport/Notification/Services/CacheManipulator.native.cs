namespace OWP.OtherSport.Notification.Services
{
    using System;
	using System.Threading.Tasks;
	using Fanex.Caching;
	using Newtonsoft.Json;
	using Submit.Models;

	public interface CacheManipulator
	{
		Task<AssignmentSnapshot> GetAssignmentSnapshot(string sessionId, DateTime eventDate);

		Task DeleteAssignmentSnapshot(string sessionId, DateTime eventDate);
	}

	public class CacheManipulatorImp : CacheManipulator
	{
		private readonly ICache cache;
        private const string KEY_OLD_OS_ASSIGNMENT_SNAPSHOT = "OWP-Old-OS-Assignment";

		public CacheManipulatorImp(ICache cache)
		{
			this.cache = cache;
		}

		public async Task<AssignmentSnapshot> GetAssignmentSnapshot(string sessionId, DateTime eventDate)
		{
			var cacheKey = $"{KEY_OLD_OS_ASSIGNMENT_SNAPSHOT}-{sessionId}-{eventDate}";
			var cacheResult = await cache.TryGetAsync<string>(cacheKey);
			var assignmentSnapshot = string.IsNullOrEmpty(cacheResult.Item2)
				? new AssignmentSnapshot()
				: JsonConvert.DeserializeObject<AssignmentSnapshot>(cacheResult.Item2);
			return assignmentSnapshot ?? new AssignmentSnapshot();
		}

		public async Task DeleteAssignmentSnapshot(string sessionId, DateTime eventDate)
		{
			var cacheKey = $"{KEY_OLD_OS_ASSIGNMENT_SNAPSHOT}-{sessionId}-{eventDate}";
			await cache.RemoveAsync(cacheKey);
		}
	}
}