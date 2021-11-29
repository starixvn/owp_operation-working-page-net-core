using OWP.Enums;
using OWP.Helpers;
using OWP.Maxbet.Constants;
using OWP.Maxbet.Models;
using System;
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

        private readonly Dictionary<int, int> maxbetMainToCornerMapper = new Dictionary<int, int>
        {
            {500_000, 18_000 },
            {400_000, 18_000 },
            {350_000, 18_000 },
            {300_000, 18_000 },
            {200_000, 18_000 },
            {180_000, 18_000 },
            {150_000,  12_000 },
            {130_000, 12_000 },
            {120_000,  12_000 },
            {110_000,  12_000 },
            {100_000, 12_000 },
            {90_000,  12_000 },
            {80_000, 7_000 },
            {70_000,  7_000 },
            {60_000,  7_000 },
            {50_000, 7_000},
            {40_000,  7_000 },
            {35_000,  7_000 },
            {30_000, 5_000 },
            {25_000,  5_000 },
            {20_000,  5_000 },
            {18_000,  5_000 },
            {15_000,  5_000 },
            {12_000, 4_000 },
            {10_000,  4_000 },
            {8_000,  4_000 },
            {7_000, 4_000 },
            {6_000, 4_000 },
            {5_000,  3_000 },
            {4_000,  3_000 },
            {3_000,  2_000 },
            {2_000,  2_000 },
        };

        private readonly Dictionary<int, int> maxbetMainToBookingMapper = new Dictionary<int, int>
        {
            {500_000,10_000},
            {400_000, 10_000},
            {350_000, 10_000},
            {300_000, 10_000},
            {200_000, 10_000},
            {180_000, 10_000},
            {150_000,6_000},
            {130_000, 6_000},
            {120_000, 6_000},
            {110_000, 6_000},
            {100_000, 6_000},
            {90_000, 3_000},
            {80_000, 3_000},
            {70_000, 3_000},
            {60_000, 3_000},
            {50_000, 3_000},
            {40_000, 3_000},
            {35_000, 3_000},
            {30_000, 3_000},
            {25_000, 3_000},
            {20_000, 3_000},
            {18_000, 3_000},
            {15_000, 3_000},
            {12_000, 3_000},
            {10_000, 3_000},
            {8_000, 3_000},
            {7_000,3_000},
            {6_000, 3_000},
            {5_000, 3_000},
            {4_000,3_000},
            {3_000, 3_000},
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

            var hasExistsOu = maxbet.Any(s => s.BettypeId == (int)EnumBettype.OverUnder);
            if (!hasExistsOu)
            {
                BuildMaxbetOverUnder(maxbet);
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
                    return BuildMaxbetNoOfCornerOrTotalBooking(maxbet, maxbetMainMatch, true);

                case SpecialBettype.BETTYPE_ID_NEXT_CORNERS:
                    return BuildMaxbetNextCornerOrBooking(maxbet, maxbetMainMatch, true);

                case SpecialBettype.BETTYPE_ID_NEXT_BOOKINGS:
                    return BuildMaxbetNextCornerOrBooking(maxbet, maxbetMainMatch);

                case SpecialBettype.BETTYPE_ID_TOTAL_BOOKINGS:
                    return BuildMaxbetNoOfCornerOrTotalBooking(maxbet, maxbetMainMatch);

                default:
                    return BuildMaxbetSender(maxbet);
            }
        }

        private MaxbetSpread BuildMaxbetNoOfCornerOrTotalBooking(List<MaxbetModel> maxbet, List<MaxbetModel> maxbetMainMatch, bool isCorner = false)
        {
            var hadicapMapper = isCorner ? maxbetMainToCornerMapper : maxbetMainToBookingMapper;
            var firstHalfHdpMapper = isCorner ? maxbetCornerMapper : maxbetBookingMapper;

            var hasExistsHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.Handicap));
            if (!hasExistsHdp)
            {
                BuildMaxbetSpecialHandicap(maxbet, maxbetMainMatch, hadicapMapper);
            }

            var hasExistsOu = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.OverUnder));
            if (!hasExistsOu)
            {
                BuildMaxbetSpecialOverUnder(maxbet, maxbetMainMatch, hadicapMapper);
            }

            var hasExistsFirstHalfHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.FirstHalfHandicap));
            if (!hasExistsFirstHalfHdp)
            {
                BuildMaxbetFirstHalfHandicap(maxbet, firstHalfHdpMapper);
            }

            var hasExistsFirstHalfOu = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.FirstHalfOverUnder));
            if (!hasExistsFirstHalfOu)
            {
                BuildMaxbetFirstHalfOverUnder(maxbet, firstHalfHdpMapper);
            }

            return BuildMaxbetSender(maxbet);
        }

        private MaxbetSpread BuildMaxbetNextCornerOrBooking(List<MaxbetModel> maxbet, List<MaxbetModel> maxbetMainMatch, bool isCorner = false)
        {
            var hadicapMapper = isCorner ? maxbetMainToCornerMapper : maxbetMainToBookingMapper;

            var hasExistsHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.Handicap));
            if (!hasExistsHdp)
            {
                BuildMaxbetSpecialHandicap(maxbet, maxbetMainMatch, hadicapMapper);
            }

            return BuildMaxbetSender(maxbet);
        }

        private void BuildMaxbetSpecialHandicap(List<MaxbetModel> maxbet, List<MaxbetModel> maxbetMainMatch, Dictionary<int, int> mapper)
        {
            var handicapMainMatch = maxbetMainMatch.FirstOrDefault(m => m.BettypeId.Equals((int)EnumBettype.Handicap)) ?? new MaxbetModel();
            var hasExistsHdpMainMatch = mapper.TryGetValue((int)handicapMainMatch.Value, out int valueFirstHdp);

            maxbet.Add(hasExistsHdpMainMatch ? new MaxbetModel
            {
                BettypeId = (int)EnumBettype.Handicap,
                Value = valueFirstHdp
            } : new MaxbetModel
            {
                BettypeId = (int)EnumBettype.Handicap,
                Value = handicapMainMatch.Value
            });
        }

        private void BuildMaxbetSpecialOverUnder(List<MaxbetModel> maxbet, List<MaxbetModel> maxbetMainMatch, Dictionary<int, int> mapper)
        {
            var handicapMainMatch = maxbetMainMatch.FirstOrDefault(m => m.BettypeId.Equals((int)EnumBettype.Handicap)) ?? new MaxbetModel();
            var hasExistsHdpMainMatch = mapper.TryGetValue((int)handicapMainMatch.Value, out int valueFirstHdp);

            maxbet.Add(hasExistsHdpMainMatch ? new MaxbetModel
            {
                BettypeId = (int)EnumBettype.OverUnder,
                Value = valueFirstHdp
            } : new MaxbetModel
            {
                BettypeId = (int)EnumBettype.OverUnder,
                Value = handicapMainMatch.Value
            });
        }

        private void BuildMaxbetOverUnder(List<MaxbetModel> maxbet)
        {
            var handicap = maxbet.FirstOrDefault(m => m.BettypeId == (int)EnumBettype.Handicap) ?? new MaxbetModel();

            maxbet.Add(new MaxbetModel
            {
                BettypeId = (int)EnumBettype.OverUnder,
                Value = handicap.Value
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