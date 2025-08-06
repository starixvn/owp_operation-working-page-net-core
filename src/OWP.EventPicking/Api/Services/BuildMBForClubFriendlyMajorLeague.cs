
namespace OWP.EventPicking.Api.Services;

public interface BuildMBForClubFriendlyMajorLeagueBuilder
{
    void Build(EventPickingModel match, List<Condition> tvList);
}

public class BuildMBForClubFriendlyMajorLeagueBuilderImp : BuildMBForClubFriendlyMajorLeagueBuilder
{
    public void Build(EventPickingModel match, List<Condition> tvList)
    {
        var (hasTv, hasIpTv) = DetermineTvIptvStatus(match, tvList);
        ApplyMajorLeagueRules(match, hasTv, hasIpTv);
    }

    private static (bool hasTv, bool hasIpTv) DetermineTvIptvStatus(EventPickingModel match, List<Condition> tvList)
    {
        // Check if TV field is valid (contains at least one item from TV List and doesn't contain "Cancel")
        var hasValidTv = !string.IsNullOrWhiteSpace(match.TV) && 
                        !match.TV.Contains("Cancel", StringComparison.OrdinalIgnoreCase) &&
                        tvList.Exists(tvCondition => IsTvConditionMatch(match.TV, tvCondition));
        
        // Check if IPTV field is valid (contains "IPTV" and doesn't contain "Cancel")
        var hasValidIptv = !string.IsNullOrWhiteSpace(match.Iptv) && 
                          !match.Iptv.Contains("Cancel", StringComparison.OrdinalIgnoreCase) &&
                          match.Iptv.Contains("IPTV", StringComparison.OrdinalIgnoreCase);
        
        // Check if Channel field is valid (not blank and doesn't contain "Cancel")
        var hasValidChannel = !string.IsNullOrWhiteSpace(match.Channel) && 
                             !match.Channel.Contains("Cancel", StringComparison.OrdinalIgnoreCase);
        
        // Apply priority rules
        if (hasValidTv)
        {
            // Rule 1: Valid TV = TV
            return (true, false);
        }
        else if (hasValidIptv || hasValidChannel)
        {
            // Rule 2: No TV but Valid IPTV = IPTV
            return (false, true);
        }
        else
        {
            // Rule 4: No TV, No IPTV, No Channel = No TV and No IPTV
            return (false, false);
        }
    }

    private static bool IsTvConditionMatch(string tvField, Condition tvCondition)
    {
        if (string.IsNullOrWhiteSpace(tvField) || string.IsNullOrWhiteSpace(tvCondition.Value))
        {
            return false;
        }

        return tvCondition.Type switch
        {
            "Contains" => tvField.Contains(tvCondition.Value, StringComparison.OrdinalIgnoreCase),
            "Equals" => tvField.Equals(tvCondition.Value, StringComparison.OrdinalIgnoreCase),
            _ => false
        };
    }

    private static void ApplyMajorLeagueRules(EventPickingModel match, bool hasTv, bool hasIpTv)
    {
        var (maxbet, spreadHdp, spreadOu, noOfHdp) = (hasTv, hasIpTv) switch
        {
            (true, _) => (40000, 0.08m, 0.1m, "4+4 lines"),
            (false, true) => (30000, 0.08m, 0.1m, "4+4 lines"),
            _ => (25000, 0.08m, 0.1m, "4+4 lines")
        };
        
        match.MaxbetHdp = maxbet;
        match.SpreadHdp = spreadHdp;
        match.SpreadOu = spreadOu;
        match.NoOfHDP = noOfHdp;
        match.IsReferenceLeagueManagement = true;
    }
}
