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
    {
        switch (type)
        {
            case DuplicateType.Round: return new RoundRule();
            case DuplicateType.OfferLine: return new OfferLinesRule();
            case DuplicateType.BetDelay: return new BetDelayRule();
            case DuplicateType.Provider: return new ProviderRule();
            case DuplicateType.RangeTime: return new RangeTimeRule();
            case DuplicateType.IPTV: return new IPTVRule();
            default: return new RemarkRule();
        }
    }
}