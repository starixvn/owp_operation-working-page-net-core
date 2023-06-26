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
        matches.ForEach(x => x.Provider = BuildProviderByOtherProvider(priorityProviders, x.OtherProvider));
    }

    private string BuildProviderByOtherProvider(List<string> priorityProviders, string otherProviderInString)
    {
        var otherProviders = otherProviderInString?.Split("/").ToList();
        foreach (var value in priorityProviders)
        {
            var provider = otherProviders?.FirstOrDefault(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(provider))
            {
                return provider;
            }
        }

        return BuildProviderTv(priorityProviders, otherProviders);
    }

    private string BuildProviderTv(List<string> priorityProviders, List<string> otherProviders)
    {
        foreach (var value in priorityProviders)
        {
            var provider = otherProviders?.FirstOrDefault(x => x.IsContains(value) && x.IsContains("TV"));
            if (!string.IsNullOrWhiteSpace(provider))
            {
                return provider;
            }
        }

        return string.Empty;
    }
}

