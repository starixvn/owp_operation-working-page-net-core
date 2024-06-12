using Fanex.Data.Repository;
using OWP.Criteria;
using OWP.Models;

namespace OWP.Services;

public interface RefResourceService
{
    Task<IEnumerable<ReferenceResource>> GetRefResourcesByName(string name);

    Task<IList<ReferenceResource>> GetAllReferenceResource();

    Task<IList<ReferenceResource>> GetResourceByNameWithNone(string resourceName);

    IList<UserLocation> GetAllLocations();
}

public class RefResourceServiceImp : RefResourceService
{
    private readonly IDynamicRepository dynamicRepository;

    public RefResourceServiceImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public IList<UserLocation> GetAllLocations()
    {
        return new List<UserLocation>
        {
            new UserLocation { LocationName = "CTI", LocationCode = "CTI" },
            new UserLocation { LocationName = "SMS", LocationCode = "SMS" },
            new UserLocation { LocationName = "WAH HCMC", LocationCode = "WAH HCMC" },
            new UserLocation { LocationName = "WAH MAL", LocationCode = "WAH MAL" },
            new UserLocation { LocationName = "WAH MNL", LocationCode = "WAH MNL" },
            new UserLocation { LocationName = "None", LocationCode = "None" }
        };
    }

    public async Task<IList<ReferenceResource>> GetAllReferenceResource()
        => (await dynamicRepository.FetchAsync<ReferenceResource>(new GetAllRefResourceCriteria())).ToList();

    public async Task<IEnumerable<ReferenceResource>> GetRefResourcesByName(string refName)
        => await dynamicRepository.FetchAsync<ReferenceResource>(new GetRefResourceByNameCriteria { RefName = refName });

    public async Task<IList<ReferenceResource>> GetResourceByNameWithNone(string resourceName)
    {
        var resources = (await GetRefResourcesByName(resourceName)).ToList();
        resources.Add(new ReferenceResource
        {
            ResourceId = 0,
            Value = "None"
        });

        return resources;
    }
}

public class GetRefResourceByNameCriteria : CriteriaBase
{
    public string RefName { get; set; }

    public override bool IsValid() => !string.IsNullOrEmpty(RefName);

    public override string GetSettingKey() => "OWP_RefResourceGetByName";
}

public static class RefName
{
    public static string MatchOffer = "MatchOffer";
}