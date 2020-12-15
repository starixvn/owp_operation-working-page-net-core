using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OWP.Enums;
using OWP.Helpers;
using OWP.Maxbet.Models;

namespace OWP.Maxbet.Builder
{
    public interface MaxbetBuilder
    {
        Task<MaxbetSpread> BuildMaxbet(int owpMatchId);
    }

    public class MaxbetBuilderImp : MaxbetBuilder
    {
        private const int numberFullyBettype = 4;

        private readonly MaxbetReposiory syncMaxbetReposiory;

        private readonly Dictionary<int, int> maxbetMapper = new Dictionary<int, int>
            {
                {400000,80000 },
                {350000,70000 },
                {200000,60000 },
                {180000,30000 },
                {150000,25000 },
                {130000,25000 },
                {120000,25000 },
                {110000,25000 },
                {100000,25000 },
                {90000,25000 },
                {80000,25000 },
                {70000,25000 },
                {60000,25000 },
                {50000,18000 },
                {40000,18000 },
                {35000,15000 },
                {30000,15000 },
                {25000,12000 },
                {20000,10000 },
                {18000,8000 },
                {15000,8000 },
                {12000,6000 },
                {10000,5000 },
                {8000,4000 },
                {7000,4000 },
                {6000,3000 },
                {5000,3000 },
                {4000,2000 },
                {3000,2000 },
                {2000,2000 }
            };

        public MaxbetBuilderImp(MaxbetReposiory syncMaxbetReposiory)
        {
            this.syncMaxbetReposiory = syncMaxbetReposiory;
        }

        public async Task<MaxbetSpread> BuildMaxbet(int owpMatchId)
        {
            var maxbet = (await syncMaxbetReposiory.GetMaxbetByOwpMatchId(owpMatchId)).ToList();

            if (!maxbet.Any())
            {
                return new MaxbetSpread();
            }

            if (maxbet.Count == numberFullyBettype)
            {
                return BuildMaxbetSender(maxbet);
            }
            var hasExistsFirstHalfHdp = maxbet.Any(s => s.BettypeId == (int)EnumBettype.FirstHalfHandicap);
            if (!hasExistsFirstHalfHdp)
            {
                BuildMaxbetFirstHalfHandicap(maxbet);
            }

            var hasExistsFirstHalfOu = maxbet.Any(s => s.BettypeId == (int)EnumBettype.FirstHalfOverUnder);
            if (!hasExistsFirstHalfOu)
            {
                BuildMaxbetFirstHalfOverUnder(maxbet);
            }

            return BuildMaxbetSender(maxbet);
        }

        public void BuildMaxbetFirstHalfOverUnder(List<MaxbetModel> maxbet)
        {
            var handicap = maxbet.FirstOrDefault(m => m.BettypeId == (int)EnumBettype.Handicap) ?? new MaxbetModel();

            int valueFirstHalfOverUnder = 0;
            var existsValueFirstHalfOverunder = handicap.Value.HasValue
                && maxbetMapper.TryGetValue((int)handicap.Value, out valueFirstHalfOverUnder);

            maxbet.Add(existsValueFirstHalfOverunder ? new MaxbetModel
            {
                BettypeId = (int)EnumBettype.FirstHalfOverUnder,
                Value = valueFirstHalfOverUnder
            } : new MaxbetModel
            {
                BettypeId = (int)EnumBettype.FirstHalfOverUnder,
                Value = handicap.Value
            });
        }

        public void BuildMaxbetFirstHalfHandicap(List<MaxbetModel> maxbet)
        {
            var handicap = maxbet.FirstOrDefault(m => m.BettypeId == (int)EnumBettype.Handicap) ?? new MaxbetModel();
            int valueFirstHalfHandicap = 0;

            var existsValueFirstHalfHandicap = handicap.Value.HasValue
                && maxbetMapper.TryGetValue((int)handicap.Value, out valueFirstHalfHandicap);

            maxbet.Add(existsValueFirstHalfHandicap ? new MaxbetModel
            {
                BettypeId = (int)EnumBettype.FirstHalfHandicap,
                Value = valueFirstHalfHandicap
            } : new MaxbetModel
            {
                BettypeId = (int)EnumBettype.FirstHalfHandicap,
                Value = handicap.Value
            });
        }

        public static MaxbetSpread BuildMaxbetSender(List<MaxbetModel> maxbet)
        {
            var handicap = MaxbetHelper.FindMaxbetByBettype(maxbet, (int)EnumBettype.Handicap) ?? new MaxbetModel();
            var overunder = MaxbetHelper.FindMaxbetByBettype(maxbet, (int)EnumBettype.OverUnder) ?? new MaxbetModel();
            var firstHalfHandicap = MaxbetHelper.FindMaxbetByBettype(maxbet, (int)EnumBettype.FirstHalfHandicap) ?? new MaxbetModel();
            var firstHalfOverUnder = MaxbetHelper.FindMaxbetByBettype(maxbet, (int)EnumBettype.FirstHalfOverUnder) ?? new MaxbetModel();
            return new MaxbetSpread
            {
                Handicap = handicap.Value,
                OverUnder = overunder.Value,
                FirstHalfHandicap = firstHalfHandicap.Value,
                FirstHalfOverUnder = firstHalfOverUnder.Value
            };
        }
    }
}