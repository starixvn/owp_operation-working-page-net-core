using Olympus.InfoSiteClient.Models;
using OWP.Extensions;

namespace OWP.Event.LiveGameAssignment.Download.Builder;

public class CoverageBuilder
{
    private readonly int[] WebsiteIdValid = new[] { 17, 18, 19, 20 };

    private readonly Dictionary<int, string> WebsitePair = new Dictionary<int, string>
    {
        {17,"RB2"},
        {18,"RTS"},
        {19,"BG"},
        {20,"BET"},
    };

    private readonly Dictionary<int, string> CoveragePair = new Dictionary<int, string>
    {
        {0,string.Empty},
        {1,"TV"}
    };

    public string Build(CommonMatchEx infositeMatch)
    {
        var coveragesMainMatch = BuildCoverageMainMatch(infositeMatch);
        var coveragesMergeMatch = BuildCoverageMergeMatch(infositeMatch);
        coveragesMainMatch.AddRange(coveragesMergeMatch);
        coveragesMainMatch = coveragesMainMatch
            .Where(c => !string.IsNullOrEmpty(c))
            .Distinct()
            .ToList();
        return string.Join("/", coveragesMainMatch);
    }

    public List<string> BuildCoverageMainMatch(CommonMatchEx infositeMatch)
    {
        List<string> coverages = new List<string>();
        var isExistsInWebsiteIdValid = WebsiteIdValid.Any(item => item == infositeMatch.website_id);
        if (isExistsInWebsiteIdValid)
        {
            var coverage = BuildCoverage(infositeMatch.coverage_id, infositeMatch.website_id);
            coverages.Add(coverage);
        }
        return coverages;
    }

    public List<string> BuildCoverageMergeMatch(CommonMatchEx infositeMatch)
    {
        List<string> coverages = new List<string>();
        infositeMatch.ls_merge_match?.ForEach(match =>
        {
            var isExistsInWebsiteIdValid = WebsiteIdValid.Any(item => item == match.website_id);
            if (isExistsInWebsiteIdValid)
            {
                var coverage = BuildCoverage(match.coverage_id, match.website_id);
                coverages.Add(coverage);
            }
        });
        return coverages;
    }

    public string BuildCoverage(int? coverageId, int? websiteId)
    {
        var website = string.Empty;
        var coveragePairHasCoverageId = coverageId.HasValue
             && CoveragePair.ContainsKey(coverageId.Value);

        if (coveragePairHasCoverageId)
        {
            website = GetValueFromDict(websiteId, WebsitePair);
        }
        var coverage = GetValueFromDict(coverageId, CoveragePair);

        return $"{website}{coverage}";
    }

    public string GetValueFromDict<T>(int? key, T dict) where T : Dictionary<int, string>
        => key.HasValue
        ? dict.GetValue(key.Value, string.Empty)
        : string.Empty;
}