using OWP.Extensions;
using OWP.Maxbet.Constants;
using OWP.Maxbet.Models;
using OWP.Maxbet.Services;

namespace OWP.Maxbet.Misc
{
    public class ETPKMatchRule
    {
        public ETPKMatchRule NextMatch { get; set; }

        public virtual async Task<MaxbetInfo> GetMaxBetETPK(MatchInfo matchInfo, int adminSiteId, int subtype)
        {
            return await NextMatch?.GetMaxBetETPK(matchInfo, adminSiteId, subtype);
        }
    }

    public class SabaMatchRule : ETPKMatchRule
    {
        private readonly SabaMatchService sabaMatchService;

        public SabaMatchRule(SabaMatchService sabaMatchService)
        {
            this.sabaMatchService = sabaMatchService;
        }

        public override async Task<MaxbetInfo> GetMaxBetETPK(MatchInfo matchInfo, int adminSiteId, int subtype)
        {
            return IsSaBaMatchETPK(matchInfo.owpLeagueName, subtype) ? await sabaMatchService.GetMaxbetInfoSaba(matchInfo.owpMatchId, adminSiteId)
                : await base.GetMaxBetETPK(matchInfo, adminSiteId, subtype);
        }

        private bool IsSaBaMatchETPK(string leagueName, int subtype)
            => IsSaBaMatch(leagueName) && subtype == SubTypeETPK.SaBaPK;

        private bool IsSaBaMatch(string leagueName)
            => leagueName.IsContainsAll("SABA", "PES", "15 Mins")
               || leagueName.IsContainsAll("PES", "20 Mins")
               || leagueName.IsContainsAll("SABA", "FIFA", "16 Mins")
               || leagueName.IsContainsAll("SABA", "FIFA", "6 Mins")
               || leagueName.IsContainsAll("M88 SABA", "FIFA", "16 Mins")
               || leagueName.IsContainsAll("SABA", "PES", "5 Mins");
    }

    public class FutsalMatchRule : ETPKMatchRule
    {
        private readonly FutsalMatchService futsalMatchService;

        public FutsalMatchRule(FutsalMatchService futsalMatchService)
        {
            this.futsalMatchService = futsalMatchService;
        }

        public override async Task<MaxbetInfo> GetMaxBetETPK(MatchInfo matchInfo, int adminSiteId, int subtype)
        {
            if (IsFutsalMatchETPK(matchInfo.owpLeagueName) && subtype == SubTypeETPK.FutsalET)
            {
                return await futsalMatchService.GetMaxBetInfoFutsalET(matchInfo.owpMatchId, adminSiteId);
            }
            if (IsFutsalMatchETPK(matchInfo.owpLeagueName) && subtype == SubTypeETPK.FutsalPK)
            {
                return await futsalMatchService.GetMaxBetInfoFutsalPEN(matchInfo.owpMatchId, adminSiteId);
            }
            return await base.GetMaxBetETPK(matchInfo, adminSiteId, subtype);
        }

        private bool IsFutsalMatchETPK(string leagueName)
            => leagueName.IsContains("FUTSAL");
    }

    public class MainMatchRule : ETPKMatchRule
    {
        private readonly MainMatchService mainMatchService;

        public MainMatchRule(MainMatchService mainMatchService)
        {
            this.mainMatchService = mainMatchService;
        }

        public override async Task<MaxbetInfo> GetMaxBetETPK(MatchInfo matchInfo, int adminSiteId, int subtype)
        {
            if (subtype == SubTypeETPK.MainMatchET)
            {
                return await mainMatchService.GetMaxbetInfoET(matchInfo.owpMatchId, adminSiteId);
            }
            if (subtype == SubTypeETPK.MainMatchPK)
            {
                return await mainMatchService.GetMaxbetInfoPEN(matchInfo.owpMatchId, adminSiteId);
            }

            return await base.GetMaxBetETPK(matchInfo, adminSiteId, subtype);
        }
    }
}