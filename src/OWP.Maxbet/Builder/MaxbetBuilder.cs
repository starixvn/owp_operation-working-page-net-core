using System;
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

        Task<MaxbetSpread> BuidlMaxbetSpecialProduct(MinorMatchInfo specialProduct, int bettypeId);
    }

    public class MaxbetBuilderImp : MaxbetBuilder
    {
        private const int numberFullyBettype = 4;

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

        private readonly Dictionary<int, Tuple<int, int, int?, int?>> maxbetSpecialMapper = new Dictionary<int, Tuple<int, int, int?, int?>>
        {
            {500_000, new (18_000,8_000,10_000,5_000)},
            {400_000, new (18_000,8_000,10_000,5_000)},
            {350_000, new (18_000,8_000,10_000,5_000)},
            {300_000, new (18_000,8_000,10_000,5_000)},
            {200_000, new (18_000,8_000,10_000,5_000)},
            {180_000, new (18_000,8_000,10_000,5_000)},
            {150_000, new (12_000,6_000,6_000,4_000)},
            {130_000, new (12_000,6_000,6_000,4_000)},
            {120_000, new (12_000,6_000,6_000,4_000)},
            {110_000, new (12_000,6_000,6_000,4_000)},
            {100_000, new (12_000,6_000,6_000,4_000)},
            {90_000, new (12_000,6_000,3_000,2_000)},
            {80_000, new (7_000,3_000,3_000,2_000)},
            {70_000, new (7_000,3_000,3_000,2_000)},
            {60_000, new (7_000,3_000,3_000,2_000)},
            {50_000, new (7_000,3_000,3_000,2_000)},
            {40_000, new (7_000,3_000,3_000,2_000)},
            {35_000, new (7_000,3_000,3_000,2_000)},
            {30_000, new (5_000,3_000,3_000,2_000)},
            {25_000, new (5_000,3_000,3_000,2_000)},
            {20_000, new (5_000,3_000,3_000,2_000)},
            {18_000, new (5_000,3_000,3_000,2_000)},
            {15_000, new (5_000,3_000,3_000,2_000)},
            {12_000, new (4_000,3_000,3_000,2_000)},
            {10_000, new (4_000,3_000,3_000,2_000)},
            {8_000, new (4_000,3_000,3_000,2_000)},
            {7_000, new (4_000,3_000,3_000,2_000)},
            {6_000, new (4_000,2_000,3_000,2_000)},
            {5_000, new (3_000,2_000,3_000,2_000)},
            {4_000, new (3_000,2_000,3_000,2_000)},
            {3_000, new (2_000,2_000,3_000,2_000)},
            {2_000, new (2_000,2_000,null,null)},
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

        public async Task<MaxbetSpread> BuidlMaxbetSpecialProduct(MinorMatchInfo specialProduct, int bettypeId)
        {
            var maxbet = (await syncMaxbetReposiory.GetMaxbetByOwpMatchId(specialProduct.MinorMatchId)).ToList();
            var maxbetMainMatch = (await syncMaxbetReposiory.GetMaxbetByOwpMatchId(specialProduct.MainMatchId)).ToList();

            switch (bettypeId)
            {
                case -3:
                    return await BuildMaxbetNoOfCorner(maxbet, maxbetMainMatch);

                case -4:
                    return await BuildMaxbetTotalBooking(maxbet, maxbetMainMatch);

                case -6:
                    return await BuildMaxbetNextCorner(maxbet, maxbetMainMatch);

                case -7:
                    return await BuildMaxbetNextBooking(maxbet, maxbetMainMatch);

                default:
                    return BuildMaxbetSender(maxbet);
            }
        }

        private async Task<MaxbetSpread> BuildMaxbetNoOfCorner(List<MaxbetModel> maxbet, List<MaxbetModel> maxbetMainMatch)
        {
            var handicapMainMatch = maxbetMainMatch.FirstOrDefault(m => m.BettypeId.Equals((int)EnumBettype.Handicap));

            //No.Of Corner //Next Corner
            var hasExistsHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.Handicap));
            if (!hasExistsHdp)
            {
                var existsValueHandicap = maxbetSpecialMapper.TryGetValue((int)handicapMainMatch.Value,
                    out Tuple<int, int, int?, int?> valueHandicap);

                maxbet.Add(existsValueHandicap ? new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.Handicap,
                    Value = valueHandicap.Item1
                } : new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.Handicap,
                    Value = handicapMainMatch.Value
                });
            }

            var hasExistsOu = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.OverUnder));
            if (!hasExistsOu)
            {
                var existsValueOverunder = maxbetSpecialMapper.TryGetValue((int)handicapMainMatch.Value,
                    out Tuple<int, int, int?, int?> valueHandicap);

                maxbet.Add(existsValueOverunder ? new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.OverUnder,
                    Value = valueHandicap.Item1
                } : new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.OverUnder,
                    Value = handicapMainMatch.Value
                });
            }

            var hasExistsFirstHalfHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.FirstHalfHandicap));
            if (!hasExistsFirstHalfHdp)
            {
                var handicap = maxbet.FirstOrDefault(m => m.BettypeId.Equals((int)EnumBettype.Handicap));
                var existsValueFirstHalfHdp = maxbetSpecialMapper.TryGetValue((int)handicapMainMatch.Value,
                    out Tuple<int, int, int?, int?> valueHandicap);

                maxbet.Add(existsValueFirstHalfHdp ? new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.FirstHalfHandicap,
                    Value = valueHandicap.Item2
                } : new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.FirstHalfHandicap,
                    Value = handicap.Value
                });
            }

            var hasExistsFirstHalfOu = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.FirstHalfOverUnder));
            if (!hasExistsFirstHalfOu)
            {
                var overUnder = maxbet.FirstOrDefault(m => m.BettypeId.Equals((int)EnumBettype.OverUnder));
                var existsValueFirstHalfHdp = maxbetSpecialMapper.TryGetValue((int)handicapMainMatch.Value,
                    out Tuple<int, int, int?, int?> valueHandicap);

                maxbet.Add(existsValueFirstHalfHdp ? new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.FirstHalfOverUnder,
                    Value = valueHandicap.Item2
                } : new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.FirstHalfOverUnder,
                    Value = overUnder.Value
                });
            }

            return BuildMaxbetSender(maxbet);
        }

        private async Task<MaxbetSpread> BuildMaxbetTotalBooking(List<MaxbetModel> maxbet, List<MaxbetModel> maxbetMainMatch)
        {
            var handicapMainMatch = maxbetMainMatch.FirstOrDefault(m => m.BettypeId.Equals((int)EnumBettype.Handicap));

            var hasExistsHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.Handicap));
            if (!hasExistsHdp)
            {
                var existsValueHandicap = maxbetSpecialMapper.TryGetValue((int)handicapMainMatch.Value,
                    out Tuple<int, int, int?, int?> valueHandicap);

                maxbet.Add((existsValueHandicap && valueHandicap.Item3.HasValue) ? new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.Handicap,
                    Value = valueHandicap.Item3.Value
                } : new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.Handicap,
                    Value = handicapMainMatch.Value
                });
            }

            var hasExistsOu = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.OverUnder));
            if (!hasExistsOu)
            {
                var existsValueOverunder = maxbetSpecialMapper.TryGetValue((int)handicapMainMatch.Value,
                    out Tuple<int, int, int?, int?> valueHandicap);

                maxbet.Add((existsValueOverunder && valueHandicap.Item3.HasValue) ? new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.OverUnder,
                    Value = valueHandicap.Item3.Value
                } : new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.OverUnder,
                    Value = handicapMainMatch.Value
                });
            }

            var hasExistsFirstHalfHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.FirstHalfHandicap));
            if (!hasExistsFirstHalfHdp)
            {
                var handicap = maxbet.FirstOrDefault(m => m.BettypeId.Equals((int)EnumBettype.Handicap));
                var existsValueFirstHalfHdp = maxbetSpecialMapper.TryGetValue((int)handicapMainMatch.Value,
                    out Tuple<int, int, int?, int?> valueHandicap);

                maxbet.Add((existsValueFirstHalfHdp && valueHandicap.Item4.HasValue) ? new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.FirstHalfHandicap,
                    Value = valueHandicap.Item4.Value
                } : new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.FirstHalfHandicap,
                    Value = handicap.Value
                });
            }

            var hasExistsFirstHalfOu = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.FirstHalfOverUnder));
            if (!hasExistsFirstHalfOu)
            {
                var overUnder = maxbet.FirstOrDefault(m => m.BettypeId.Equals((int)EnumBettype.OverUnder));
                var existsValueFirstHalfHdp = maxbetSpecialMapper.TryGetValue((int)handicapMainMatch.Value,
                    out Tuple<int, int, int?, int?> valueHandicap);

                maxbet.Add((hasExistsFirstHalfOu && valueHandicap.Item4.HasValue) ? new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.FirstHalfOverUnder,
                    Value = valueHandicap.Item4.Value
                } : new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.FirstHalfOverUnder,
                    Value = overUnder.Value
                });
            }

            return BuildMaxbetSender(maxbet);
        }

        private async Task<MaxbetSpread> BuildMaxbetNextCorner(List<MaxbetModel> maxbet, List<MaxbetModel> maxbetMainMatch)
        {
            var handicapMainMatch = maxbetMainMatch.FirstOrDefault(m => m.BettypeId.Equals((int)EnumBettype.Handicap));

            var hasExistsHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.Handicap));
            if (!hasExistsHdp)
            {
                var existsValueHandicap = maxbetSpecialMapper.TryGetValue((int)handicapMainMatch.Value,
                    out Tuple<int, int, int?, int?> valueHandicap);

                maxbet.Add(existsValueHandicap ? new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.Handicap,
                    Value = valueHandicap.Item1
                } : new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.Handicap,
                    Value = handicapMainMatch.Value
                });
            }

            return BuildMaxbetSender(maxbet);
        }

        private async Task<MaxbetSpread> BuildMaxbetNextBooking(List<MaxbetModel> maxbet, List<MaxbetModel> maxbetMainMatch)
        {
            var handicapMainMatch = maxbetMainMatch.FirstOrDefault(m => m.BettypeId.Equals((int)EnumBettype.Handicap));

            var hasExistsHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.Handicap));
            if (!hasExistsHdp)
            {
                var existsValueHandicap = maxbetSpecialMapper.TryGetValue((int)handicapMainMatch.Value,
                    out Tuple<int, int, int?, int?> valueHandicap);

                maxbet.Add((existsValueHandicap && valueHandicap.Item3.HasValue) ? new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.Handicap,
                    Value = valueHandicap.Item3.Value
                } : new MaxbetModel
                {
                    BettypeId = (int)EnumBettype.Handicap,
                    Value = handicapMainMatch.Value
                });
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