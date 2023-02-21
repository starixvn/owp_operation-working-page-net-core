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
    private const string KeyAssignmentConfiguration = "OWP-assignment-configuration";
    private const int CacheExpiredInDays = 36500;
    private readonly ICache cache;
    private readonly ProductAssignmentRepository repository;
    private readonly RefResourceService refResourceService;

    public const string UserRole = "UserRole";
    public const string Section = "Section";
    public const string Location = "LocationCode";
    public const string MaxMultipleGame = "MultipleGames";
    public const int SectionLiveAssistantVn = 2;
    public const int SectionLiveAssistantPh = 30;
    public const int SectionLiveTrading = 3;
    public const int LocationCti = 2;
    public const int LocationSms = 1;

    public readonly int[] SectionId = {
        SectionLiveAssistantVn,
        SectionLiveAssistantPh,
        SectionLiveTrading
    };

    public readonly int[] LocationId = {
        LocationCti,
        LocationSms
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
        var (_, value) = await cache.TryGetAsync<string>(KeyAssignmentConfiguration);

        var assignments = string.IsNullOrEmpty(value)
            ? new LastUpdateInfo()
            : JsonConvert.DeserializeObject<LastUpdateInfo>(value);

        return assignments;
    }

    public async Task<object> GetDropdownOption()
    {
        var allResource = await refResourceService.GetAllReferenceResource();
        var roleOption = allResource.Where(r => string.Equals(r.Name, UserRole)).ToList();
        var sectionOption = allResource.Where(r => string.Equals(r.Name, Section) && SectionId.Any(id => id == r.ResourceId));
        var locationOption = allResource.Where(r => string.Equals(r.Name, Location) && LocationId.Any(id => id == r.ResourceId));
        var levelOption = await BuildLevelBySectionId();
        var mainBettypeOption = await BuildMainBettypeBySectionId();
        var multipleMaxGamesOption = allResource.Where(r => string.Equals(r.Name, MaxMultipleGame)).ToList();
        AddEmptyOption(roleOption);
        AddEmptyOption(multipleMaxGamesOption);
        return new
        {
            roleOption,
            sectionOption,
            locationOption,
            levelOption,
            mainBettypeOption,
            multipleMaxGamesOption
        };
    }

    public async Task<IList<UserInfo>> QueryUser(QueryUserInfo queryInfo)
    {
        var infoUser = await repository.QueryUser(queryInfo);
        foreach (var item in infoUser)
        {
            item.BetTypePreferencesValues = await BuildBetTypePreferencesValueBySection(item);
        }

        return infoUser.ToList();
    }

    public async Task<List<MainBetTypePreferences>> BuildBetTypePreferencesValueBySection(UserInfo user)
    {
        var result = new List<MainBetTypePreferences>();
        if (user.BetTypePreferencesId == null)
        {
            return result;
        }
        var levelOption = await repository.GetMainBettypeBySectionId(user.SectionId);
        var betTypeArr = user.BetTypePreferencesId.Split(',').Select(x => int.Parse(x.Trim())).ToList();
        result = levelOption.Where(item => betTypeArr.Contains(item.ResourceId)).Select(x => new MainBetTypePreferences
        {
            Label = x.Value,
            Value = x.ResourceId
        }).ToList();

        return result;
    }

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
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CacheExpiredInDays));
        var assignmentSnapshotInJson = JsonConvert.SerializeObject(info);
        await cache.SetAsync(KeyAssignmentConfiguration, assignmentSnapshotInJson, cachePolicy);
    }

    private async Task<object> BuildLevelBySectionId()
    {
        var levelOptionDefault = await repository.GetLevelBySectionId(null);
        var levelOptionLiveTrading = await repository.GetLevelBySectionId(SectionLiveTrading);
        var levelOptionLiveAssistant = await repository.GetLevelBySectionId(SectionLiveAssistantPh);
        AddEmptyOption(levelOptionDefault);
        AddEmptyOption(levelOptionLiveTrading);
        AddEmptyOption(levelOptionLiveAssistant);
        return new
        {
            levelOptionDefault,
            levelOptionLiveTrading,
            levelOptionLiveAssistant
        };
    }

    private static void AddEmptyOption(ICollection<ReferenceResource> options)
        => options.Add(new ReferenceResource
        {
            ResourceId = 0,
            Value = string.Empty
        });

    private async Task<object> BuildMainBettypeBySectionId()
    {
        var mainBettypeOptionDefault = await repository.GetMainBettypeBySectionId(null);
        var mainBettypeOptionLiveTrading = await repository.GetMainBettypeBySectionId(SectionLiveTrading);
        var mainBettypeOptionLiveAssistant = await repository.GetMainBettypeBySectionId(SectionLiveAssistantPh);
        return new
        {
            mainBettypeOptionDefault,
            mainBettypeOptionLiveTrading,
            mainBettypeOptionLiveAssistant
        };
    }
}