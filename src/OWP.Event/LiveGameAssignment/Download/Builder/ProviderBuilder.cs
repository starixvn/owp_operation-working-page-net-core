namespace OWP.Event.LiveGameAssignment.Download.Builder;

using System.Linq;

public static class ProviderBuilder
{
    public static string Build(string providers)
       => string.IsNullOrWhiteSpace(providers)
            ? string.Empty
            : providers.Replace("/", string.Empty);

    public static string GetProvidersFormat(string providers)
    {
        if (string.IsNullOrWhiteSpace(providers))
        {
            return string.Empty;
        }

        var providerFilter = providers
                                .Replace("/", string.Empty)
                                    .Split(',')
                                        .Where(a => !string.IsNullOrEmpty(a));
        return string.Join("/", providerFilter);
    }
}