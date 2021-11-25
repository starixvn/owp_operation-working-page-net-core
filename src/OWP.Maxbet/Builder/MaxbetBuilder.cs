using OWP.Enums;
using OWP.Helpers;
using OWP.Maxbet.Constants;
using OWP.Maxbet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.Maxbet.Builder
{
    public interface MaxbetBuilder
    {
        Task<MaxbetSpread> BuildMaxbet(int owpMatchId);

        Task<MaxbetSpread> BuidlMaxbetSpecialProduct(MinorMatchInfo specialProduct, int bettypeId);
    }

    public class MaxbetBuilderImp : MaxbetBuilder
    {
        private readonly MaxbetReposiory syncMaxbetReposiory;

        private readonly Dictionary<int, int> maxbetMapper = new Dictionary<int, int>
            {
                {500_000,120_000 },
                {400_000,120_000 },
                {350_000,100_000 },
                {300_000,100_000 },
                {200_000,100_000 },
                {180_000,50_000 },
                {150_000,40_000 },
                {130_000,40_000 },
                {120_000,40_000 },
                {110_000,40_000 },
                {100_000,40_000 },
                {90_000,40_000 },
                {80_000,30_000 },
                {70_000,30_000 },
                {60_000,25_000 },
                {50_000,25_000 },
                {40_000,20_000 },
                {35_000,18_000 },
                {30_000,15_000 },
                {25_000,12_000 },
                {20_000,10_000 },
                {18_000,10_000 },
                {15_000,8_000 },
                {12_000,6_000 },
                {10_000,5_000 },
                {8_000,4_000 },
                {7_000,4_000 },
                {6_000,3_000 },
                {5_000,3_000 },
                {4_000,2_000 },
                {3_000,2_000 },
                {2_000,2_000 }
            };

        private readonly Dictionary<int, int> maxbetCornerMapper = new Dictionary<int, int>
        {
            {18_000,8_000},
            {12_000,6_000},
            {7_000,3_000},
            {5_000,3_000},
            {4_000,3_000},
            {3_000,2_000},
            {2_000,2_000},
        };

        private readonly Dictionary<int, int> maxbetBookingMapper = new Dictionary<int, int>
        {
            {10_000,5_000},
            {6_000,4_000},
            {3_000,2_000},
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

            if (maxbet.Count == SpecialBettype.NUMBER_FULL_BETTYPE)
            {
                return BuildMaxbetSender(maxbet);
            }
            var hasExistsFirstHalfHdp = maxbet.Any(s => s.BettypeId == (int)EnumBettype.FirstHalfHandicap);
            if (!hasExistsFirstHalfHdp)
            {
                BuildMaxbetFirstHalfHandicap(maxbet, maxbetMapper);
            }

            var hasExistsFirstHalfOu = maxbet.Any(s => s.BettypeId == (int)EnumBettype.FirstHalfOverUnder);
            if (!hasExistsFirstHalfOu)
            {
                BuildMaxbetFirstHalfOverUnder(maxbet, maxbetMapper);
            }

            return BuildMaxbetSender(maxbet);
        }

        public async Task<MaxbetSpread> BuidlMaxbetSpecialProduct(MinorMatchInfo specialProduct, int bettypeId)
        {
            var maxbet = (await syncMaxbetReposiory.GetMaxbetByOwpMatchId(specialProduct.MinorMatchId)).ToList();
            var maxbetMainMatch = (await syncMaxbetReposiory.GetMaxbetByOwpMatchId(specialProduct.MainMatchId)).ToList();

            switch (bettypeId)
            {
                case SpecialBettype.BETTYPE_ID_NO_OF_CORNERS:
                    return BuildMaxbetNoOfCornerOrTotalBooking(maxbet, maxbetMainMatch, maxbetCornerMapper);

                case SpecialBettype.BETTYPE_ID_NEXT_CORNERS:
                case SpecialBettype.BETTYPE_ID_NEXT_BOOKINGS:
                    return BuildMaxbetNextCornerOrBooking(maxbet, maxbetMainMatch);

                case SpecialBettype.BETTYPE_ID_TOTAL_BOOKINGS:
                    return BuildMaxbetNoOfCornerOrTotalBooking(maxbet, maxbetMainMatch, maxbetBookingMapper);

                default:
                    return BuildMaxbetSender(maxbet);
            }
        }

        private MaxbetSpread BuildMaxbetNoOfCornerOrTotalBooking(List<MaxbetModel> maxbet, List<MaxbetModel> maxbetMainMatch, Dictionary<int, int> mapper)
        {
            var hasExistsHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.Handicap));
            if (!hasExistsHdp)
            {
                BuildMaxbetSpecialHandicap(maxbet, maxbetMainMatch);
            }

            var hasExistsOu = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.OverUnder));
            if (!hasExistsOu)
            {
                BuildMaxbetSpecialOverUnder(maxbet);
            }

            var hasExistsFirstHalfHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.FirstHalfHandicap));
            if (!hasExistsFirstHalfHdp)
            {
                BuildMaxbetFirstHalfHandicap(maxbet, mapper);
            }

            var hasExistsFirstHalfOu = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.FirstHalfOverUnder));
            if (!hasExistsFirstHalfOu)
            {
                BuildMaxbetFirstHalfOverUnder(maxbet, mapper);
            }

            return BuildMaxbetSender(maxbet);
        }

        private MaxbetSpread BuildMaxbetNextCornerOrBooking(List<MaxbetModel> maxbet, List<MaxbetModel> maxbetMainMatch)
        {
            var hasExistsHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.Handicap));
            if (!hasExistsHdp)
            {
                BuildMaxbetSpecialHandicap(maxbet, maxbetMainMatch);
            }

            return BuildMaxbetSender(maxbet);
        }

        private void BuildMaxbetSpecialHandicap(List<MaxbetModel> maxbet, List<MaxbetModel> maxbetMainMatch)
        {
            var handicapMainMatch = maxbetMainMatch.FirstOrDefault(m => m.BettypeId.Equals((int)EnumBettype.Handicap));

            maxbet.Add(new MaxbetModel
            {
                BettypeId = (int)EnumBettype.Handicap,
                Value = handicapMainMatch.Value
            });
        }

        private void BuildMaxbetSpecialOverUnder(List<MaxbetModel> maxbet)
        {
            var handicapMainMatch = maxbet.FirstOrDefault(m => m.BettypeId.Equals((int)EnumBettype.Handicap));
            maxbet.Add(new MaxbetModel
            {
                BettypeId = (int)EnumBettype.OverUnder,
                Value = handicapMainMatch.Value
            });
        }

        public void BuildMaxbetFirstHalfOverUnder(List<MaxbetModel> maxbet, Dictionary<int, int> mapper)
        {
            var handicap = maxbet.FirstOrDefault(m => m.BettypeId == (int)EnumBettype.Handicap) ?? new MaxbetModel();

            int valueFirstHalfOverUnder = 0;
            var existsValueFirstHalfOverunder = handicap.Value.HasValue
                && mapper.TryGetValue((int)handicap.Value, out valueFirstHalfOverUnder);

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

        public void BuildMaxbetFirstHalfHandicap(List<MaxbetModel> maxbet, Dictionary<int, int> mapper)
        {
            var handicap = maxbet.FirstOrDefault(m => m.BettypeId == (int)EnumBettype.Handicap) ?? new MaxbetModel();
            int valueFirstHalfHandicap = 0;

            var existsValueFirstHalfHandicap = handicap.Value.HasValue
                && mapper.TryGetValue((int)handicap.Value, out valueFirstHalfHandicap);

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