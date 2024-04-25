using OWP.EventPicking.Helper;
using OWP.EventPicking.Models;
using OWP.Extensions;
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

namespace OWP.EventPicking.Services;

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

