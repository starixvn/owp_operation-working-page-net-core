using Fanex.Data.Repository;
using OWP.Models;

namespace OWP.Services;

public interface RefResourceService
{
    Task<IEnumerable<ReferenceResource>> GetRefResourcesByName(string name);
}

public class RefResourceServiceImp : RefResourceService
{
    private readonly IDynamicRepository dynamicRepository;

    public RefResourceServiceImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<IEnumerable<ReferenceResource>> GetRefResourcesByName(string refName)
        => await dynamicRepository.FetchAsync<ReferenceResource>(new GetRefResourceByNameCriteria { RefName = refName });
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