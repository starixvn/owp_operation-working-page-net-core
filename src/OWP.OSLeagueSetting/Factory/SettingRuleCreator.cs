using OWP.OSLeagueSetting.Constants;
using OWP.OSLeagueSetting.Misc;

namespace OWP.OSLeagueSetting.Factory;

public interface SettingRuleCreator
{
    public DuplicateRule Create(DuplicateType type);
}

public class SettingRuleCreatorImp : SettingRuleCreator
{
    public DuplicateRule Create(DuplicateType type)
        => type switch
        {
            DuplicateType.Round => new RoundRule(),
            DuplicateType.OfferLine => new OfferLinesRule(),
            DuplicateType.BetDelay => new BetDelayRule(),
            DuplicateType.Provider => new ProviderRule(),
            DuplicateType.RangeTime => new RangeTimeRule(),
            DuplicateType.IPTV => new IPTVRule(),
            _ => new RemarkRule()
        };
}