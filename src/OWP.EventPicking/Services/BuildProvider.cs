using OWP.EventPicking.Models;

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
        var priorityProviders = await GetAllProviders();
        matches.ForEach(x => x.Provider = BuildProviderByOtherProvider(priorityProviders, x.OtherProvider));
    }

    private async Task<List<string>> GetAllProviders()
    {
        var providers = await repository.GetProviderArrangement();
        var tvProvider = providers.Select(x => x + "TV");
        providers.AddRange(tvProvider.ToList());
        return providers;
    }

    private string BuildProviderByOtherProvider(List<string> priorityProviders, string otherProvider)
    {
        var otherProviderList = otherProvider?.Split("/");
        foreach (var value in priorityProviders)
        {
            var provider = otherProviderList?.FirstOrDefault(x => x == value);
            if (!string.IsNullOrWhiteSpace(provider))
            {
                return provider;
            }
        }

        return string.Empty;
    }
}

