using OWP.Maxbet.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.Maxbet.Builder
{
    public interface SpecialProductMaxbetBuilder
    {
        Task<MaxbetSpread> BuildMaxbet(int owpMatchId);

        Task<MaxbetSpread> BuildMaxbetXMins(int owpMatchId);

        Task<MaxbetSpread> BuildMaxbetSpecialProduct(MinorMatchInfo specialProduct, int bettypeId);
    }

    public class SpecialProductMaxbetBuilderImp : SpecialProductMaxbetBuilder
    {
        private readonly MaxbetReposiory syncMaxbetReposiory;
        private readonly MaxbetBuilder maxbetBuilder;

        public SpecialProductMaxbetBuilderImp(MaxbetReposiory syncMaxbetReposiory, MaxbetBuilder maxbetBuilder)
        {
            this.syncMaxbetReposiory = syncMaxbetReposiory;
            this.maxbetBuilder = maxbetBuilder;
        }

        public async Task<MaxbetSpread> BuildMaxbet(int owpMatchId)
        {
            var maxbet = await syncMaxbetReposiory.GetMaxbetByOwpMatchId(owpMatchId);
            return !maxbet.Any()
                ? new()
                : MaxbetBuilderImp.BuildMaxbetSender(maxbet.ToList());
        }

        public async Task<MaxbetSpread> BuildMaxbetSpecialProduct(MinorMatchInfo specialProduct, int bettypeId)
        {
            return await maxbetBuilder.BuidlMaxbetSpecialProduct(specialProduct, bettypeId);
        }

        public async Task<MaxbetSpread> BuildMaxbetXMins(int owpMatchId)
        {
            var maxbet = await syncMaxbetReposiory.GetMaxbetXMins(owpMatchId);
            var maxbetHdp = maxbet?.FirstOrDefault();
            return new()
            {
                Handicap = maxbetHdp,
                OverUnder = maxbetHdp
            };
        }
    }
}