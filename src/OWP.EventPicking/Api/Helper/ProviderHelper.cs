namespace OWP.EventPicking.Api.Helper;
public static class ProviderHelper
{
    public static string BuildProviderByOtherProvider(List<string> priorityProviders, string otherProviderInString)
    {
        var otherProviders = otherProviderInString?.Split("/").ToList();
        foreach (var value in priorityProviders)
        {
            var provider = otherProviders?.Find(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(provider))
            {
                return provider;
            }
        }

        return BuildProviderTv(priorityProviders, otherProviders);
    }

    public static string BuildProviderTv(List<string> priorityProviders, List<string> otherProviders)
    {
        foreach (var value in priorityProviders)
        {
            var provider = otherProviders?.Find(x => x.IsContains(value) && x.IsContains("TV"));
            if (!string.IsNullOrWhiteSpace(provider))
            {
                return provider;
            }
        }

        return string.Empty;
    }

    public static string SortOtherProvider(List<string> priorityProviders, string otherProviderInString)
    {
        if (!string.IsNullOrEmpty(otherProviderInString))
        {
            var sortedOtherProviderList = otherProviderInString.Split('/')
                .OrderBy(p =>
                {
                    var baseProvider = p.EndsWith("TV") ? p.Substring(0, p.Length - 2) : p;
                    var index = priorityProviders.IndexOf(baseProvider);
                    return priorityProviders.Contains(baseProvider) && p == baseProvider ? index : index + priorityProviders.Count;
                })
                .ToList();

            return string.Join("/", sortedOtherProviderList);
        }

        return string.Empty;
    }
}
