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

public class RefResourceServiceImp(IDynamicRepository dynamicRepository) : RefResourceService
{
    public IList<UserLocation> GetAllLocations()
    {
        return new List<UserLocation>
        {
            new() { LocationName = "CSC", LocationCode = "CSC" },
            new() { LocationName = "CTI", LocationCode = "CTI" },
            new() { LocationName = "DBS", LocationCode = "DBS" },
            new() { LocationName = "SMS", LocationCode = "SMS" },
            new() { LocationName = "WAH HCMC", LocationCode = "WAH HCMC" },
            new() { LocationName = "WAH MAL", LocationCode = "WAH MAL" },
            new() { LocationName = "WAH MNL", LocationCode = "WAH MNL" },
            new() { LocationName = "WGI", LocationCode = "WGI" },
            new() { LocationName = "XPC", LocationCode = "XPC" },
            new() { LocationName = "None", LocationCode = "None" }
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