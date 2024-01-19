using Olympus.InfoSiteClient.Models;

namespace OWP.Event.LiveGameAssignment.Download.Builder;

public class OtherProviderBuilder
{
    public static string Build(CommonMatchEx infositeMatch)
    {
        var otherProviderItems = new List<string>();
        var coverageBuilder = new CoverageBuilder();
        otherProviderItems.Add(TVBuilder.Build(infositeMatch));
        otherProviderItems.Add(coverageBuilder.Build(infositeMatch));
        otherProviderItems.Add(ProviderBuilder.GetProvidersFormat(infositeMatch.ext_info?.provider));

        return string.Join("/", otherProviderItems.Where(item => !string.IsNullOrEmpty(item)));
    }
}