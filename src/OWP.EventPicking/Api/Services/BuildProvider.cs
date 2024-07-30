using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Models;

// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

namespace OWP.EventPicking.Api.Services;

public interface BuildProvider
{
    Task Build(List<EventPickingModel> matches);
}

public class BuildProviderImp : BuildProvider
{
    private readonly Repository repository;

    public BuildProviderImp(Repository repository)
    {
        this.repository = repository;
    }

    public async Task Build(List<EventPickingModel> matches)
    {
        var priorityProviders = await repository.GetProviderArrangement();
        matches.ForEach(x => x.Provider = ProviderHelper.BuildProviderByOtherProvider(priorityProviders, x.OtherProvider));
    }
}

