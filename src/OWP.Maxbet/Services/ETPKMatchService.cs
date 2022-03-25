using OWP.Maxbet.Constants;
using OWP.Maxbet.Misc;
using OWP.Maxbet.Models;

namespace OWP.Maxbet.Services
{
    public interface ETPKMatchService
    {
        Task<MaxbetInfo> GetMaxBetETPK(int adminSiteId, int subtype);
    }

    public class ETPKMatchServiceImp : ETPKMatchService
    {
        private readonly FutsalMatchService futsalMatchService;
        private readonly MainMatchService mainMatchService;
        private readonly SabaMatchService sabaMatchService;
        private readonly SpecialProductService specialProductService;
        private readonly MaxbetRepository maxbetRepository;

        public ETPKMatchServiceImp(FutsalMatchService futsalMatchService,
            MainMatchService mainMatchService,
            SabaMatchService sabaMatchService,
            SpecialProductService specialProductService,
            MaxbetRepository maxbetRepository)
        {
            this.mainMatchService = mainMatchService;
            this.futsalMatchService = futsalMatchService;
            this.specialProductService = specialProductService;
            this.sabaMatchService = sabaMatchService;
            this.maxbetRepository = maxbetRepository;
        }

        public async Task<MaxbetInfo> GetMaxBetETPK(int adminSiteId, int subtype)
        {
            var bettypeSpecial = GetSubtypeSpecial(subtype);

            if (bettypeSpecial.HasValue)
            {
                return await specialProductService.GetMaxbetInfoET(adminSiteId, bettypeSpecial.Value);
            }

            var matchInfo = await maxbetRepository.GetMatchInfoByAdminSiteId(adminSiteId);

            return await GetMaxBetETPKCommon(matchInfo, adminSiteId, subtype);
        }

        private async Task<MaxbetInfo> GetMaxBetETPKCommon(MatchInfo matchInfo, int adminSiteId, int subtype)
        {
            var saBaMatch = new SabaMatchRule(sabaMatchService);
            var futsalMatch = new FutsalMatchRule(futsalMatchService);
            var mainMatch = new MainMatchRule(mainMatchService);
            saBaMatch.NextMatch = futsalMatch;
            futsalMatch.NextMatch = mainMatch;
            return await saBaMatch.GetMaxBetETPK(matchInfo, adminSiteId, subtype);
        }

        private static int? GetSubtypeSpecial(int subtype)
         => SubTypeETPK.SpecialSubType.ContainsKey(subtype)
            ? SubTypeETPK.SpecialSubType[subtype] : null;
    }
}