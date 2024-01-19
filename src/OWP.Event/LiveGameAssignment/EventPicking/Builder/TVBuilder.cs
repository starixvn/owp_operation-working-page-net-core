using Olympus.InfoSiteClient.Models;

namespace OWP.Event.LiveGameAssignment.EventPicking.Builder;

public static class TVBuilder
{
    public static string Build(CommonMatchEx infositeMatch)
    {
        var extInfo = infositeMatch.ext_info;
        var condition = new string[4]
        {
            extInfo.ip_tv,
            extInfo.local_tv,
            extInfo.tv_live,
            WatchBetBuilder.Build(extInfo)
        };

        var websiteIdValid = new int[4] { 17, 18, 19, 20 };

        return websiteIdValid.Any(w => w == infositeMatch.website_id)
            && infositeMatch.coverage_id == 1
            && condition.Any(c => !string.IsNullOrEmpty(c))
                ? "TV"
                : string.Empty;
    }
}