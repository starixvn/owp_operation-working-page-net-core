
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
        var hasValidTv = !string.IsNullOrWhiteSpace(match.TV) && 
                        !match.TV.Contains("Cancel", StringComparison.OrdinalIgnoreCase) &&
                        tvList.Exists(tvCondition => IsTvConditionMatch(match.TV, tvCondition));
        
        var hasValidIptv = !string.IsNullOrWhiteSpace(match.Iptv) && 
                          !match.Iptv.Contains("Cancel", StringComparison.OrdinalIgnoreCase) &&
                          match.Iptv.Contains("IPTV", StringComparison.OrdinalIgnoreCase);
        
        var hasValidChannel = !string.IsNullOrWhiteSpace(match.Channel) && 
                             !match.Channel.Contains("Cancel", StringComparison.OrdinalIgnoreCase);
        
        if (hasValidTv)
        {
            return (true, false);
        }
        else if (hasValidIptv || hasValidChannel)
        {
            return (false, true);
        }
        else
        {
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
