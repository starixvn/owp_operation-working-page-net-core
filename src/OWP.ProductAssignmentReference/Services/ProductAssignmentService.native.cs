namespace OWP.ProductAssignmentReference.Services;

using Fanex.Caching;
using Models;
using Newtonsoft.Json;
using OWP.Models;
using OWP.Services;

public interface ProductAssignmentService
{
    Task<IList<UserInfo>> QueryUser(QueryUserInfo queryInfo);

    Task UpdateUserInfo(UserInfoEditor info);

    Task<LastUpdateInfo> GetLastUpdate();

        Task<object> GetDropdownOption();
}

public class ProductAssignmentServiceImp : ProductAssignmentService
{
    private const string KEY_ASSIGNMENT_CONFIGURATION = "OWP-assignment-configuration";
    private const int CACHE_EXPIRED_IN_DAYS = 36500;
    private readonly ICache cache;
    private readonly ProductAssignmentRepository repository;
        private readonly RefResourceService refResourceService;

        public const string USER_ROLE = "UserRole";
        public const string SECTION = "Section";
        public const string LOCATION = "LocationCode";
        public const string LEVEL = "UserLevel";
        public const string MAIN_BETTYPE = "BetTypePreferences";
        public const string MAX_MULTIPLE_GAME = "MultipleGames";
        public const int SECTION_LIVE_ASSISTANT_VN = 2;
        public const int SECTION_LIVE_ASSISTANT_PH = 30;
        public const int SECTION_LIVE_TRADING = 3;
        public const int LOCATION_CTI = 2;
        public const int LOCATION_SMS = 1;

        public readonly int[] sectionId = new int[] {
            SECTION_LIVE_ASSISTANT_VN,
            SECTION_LIVE_ASSISTANT_PH,
            SECTION_LIVE_TRADING
            };

        public readonly int[] locationId = new int[] {
            LOCATION_CTI,
            LOCATION_SMS
            };

    public ProductAssignmentServiceImp(
        ProductAssignmentRepository repository,
            ICache cache,
            RefResourceService refResourceService)
    {
        this.repository = repository;
        this.cache = cache;
            this.refResourceService = refResourceService;
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

        public async Task<object> GetDropdownOption()
        {
            var allResource = await refResourceService.GetAllReferenceResource(); ;
            var roleOption = allResource.Where(r => string.Equals(r.Name, USER_ROLE));
            var sectionOption = allResource.Where(r => string.Equals(r.Name, SECTION) && sectionId.Any(id => id == r.ResourceId));
            var locationOption = allResource.Where(r => string.Equals(r.Name, LOCATION) && locationId.Any(id => id == r.ResourceId));
            var levelOption = await BuildLevelBySectionId();
            var mainBettypeOption = await BuildMainBettypeBySectionId();
            var multipleMaxGames = allResource.Where(r => string.Equals(r.Name, MAX_MULTIPLE_GAME));
            return new
            {
                roleOption,
                sectionOption,
                locationOption,
                levelOption,
                mainBettypeOption
            };
        }

        private async Task<object> BuildMainBettypeBySectionId()
        {
            var levelOptionDefault = await repository.GetMainBettypeBySectionId(null);
            var levelOptionLiveTrading = await repository.GetMainBettypeBySectionId(SECTION_LIVE_TRADING);
            var levelOptionLiveAssistant = await repository.GetMainBettypeBySectionId(SECTION_LIVE_ASSISTANT_PH);
            return new
            {
                levelOptionDefault,
                levelOptionLiveTrading,
                levelOptionLiveAssistant
            };
        }

        private async Task<object> BuildLevelBySectionId()
        {
            var levelOptionDefault = await repository.GetLevelBySectionId(null);
            var levelOptionLiveTrading = await repository.GetLevelBySectionId(SECTION_LIVE_TRADING);
            var levelOptionLiveAssistant = await repository.GetLevelBySectionId(SECTION_LIVE_ASSISTANT_PH);
            return new
            {
                levelOptionDefault,
                levelOptionLiveTrading,
                levelOptionLiveAssistant
            };
        }

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