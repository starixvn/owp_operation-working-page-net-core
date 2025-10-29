
namespace OWP.EventPicking.Api.Services;

public interface BuildMBForClubFriendlyMajorLeagueBuilder
{
    void Build(EventPickingModel match, bool hasTv, bool hasIpTv);
}

public class BuildMBForClubFriendlyMajorLeagueBuilderImp : BuildMBForClubFriendlyMajorLeagueBuilder
{
    public void Build(EventPickingModel match, bool hasTv, bool hasIpTv)
    {
        ApplyMajorLeagueRules(match, hasTv, hasIpTv);
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
        match.MaxbetOu = maxbet;
        match.SpreadHdp = spreadHdp;
        match.SpreadOu = spreadOu;
        match.NoOfHDP = noOfHdp;
        match.IsReferenceLeagueManagement = true;
    }
}
