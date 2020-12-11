namespace OWP.Maxbet.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using MediatR;
    using OWP.Maxbet.Criterias;
    using OWP.Maxbet.Models;
    using OWP.Enums;
    using OWP.Helpers;

    public class GetMaxbetCN88Handler
    {
        public class Request : IRequest<MaxbetSpread>
        {
            public int OWPMatchId { get; set; }
        }

        public class Handler : IRequestHandler<Request, MaxbetSpread>
        {
            private static readonly ReadOnlyDictionary<int, IList<MaxbetModel>> cn88Dict = new ReadOnlyDictionary<int, IList<MaxbetModel>>(
                new Dictionary<int, IList<MaxbetModel>>
                {
                    { 400000, new  List<MaxbetModel> {
                        new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 200000},
                        new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 60000}
                        }
                    },
                    { 350000, new  List<MaxbetModel> {
                        new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 180000},
                        new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 40000}
                        }
                    },
                    { 200000, new  List<MaxbetModel> {
                        new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 150000},
                        new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 40000}
                        }
                    },
                    { 180000, new  List<MaxbetModel> {
                        new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 130000},
                        new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 40000}
                        }
                    },
                    { 150000, new  List<MaxbetModel> {
                        new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 120000},
                        new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 40000}
                        }
                    },
                    { 130000, new  List<MaxbetModel> {
                        new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 110000},
                        new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 40000}
                        }
                    },
                    { 120000, new  List<MaxbetModel> {
                        new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 100000},
                        new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 40000}
                        }
                    },
                    { 110000, new  List<MaxbetModel> {
                        new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 90000},
                        new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 40000}
                        }
                    },
                    { 100000, new  List<MaxbetModel> {
                        new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 80000},
                        new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 30000}
                        }
                    },
                    { 90000, new  List<MaxbetModel> {
                        new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 70000},
                        new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 30000}
                        }
                    },
                    { 80000, new  List<MaxbetModel> {
                        new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 60000},
                        new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 24000}
                        }
                    },
                    { 70000, new  List<MaxbetModel> {
                        new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 55000},
                        new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 18000}
                        }
                    },
                    { 60000, new  List<MaxbetModel> {
                        new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 50000},
                        new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 18000}
                        }
                    },
                    { 50000, new  List<MaxbetModel> {
                        new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 40000},
                        new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 18000}
                        }
                    },
                }
            );

            private readonly IDynamicRepository dynamicRepository;

            public Handler(IDynamicRepository dynamicRepository)
            {
                this.dynamicRepository = dynamicRepository;
            }

            public async Task<MaxbetSpread> Handle(Request request, CancellationToken cancellationToken)
            {
                var cn88Maxbets = await GetCN88Maxbet(request.OWPMatchId);
                var novaMaxbets = await GetNovaMaxbet(request.OWPMatchId);
                var novaFTHDP = novaMaxbets.FirstOrDefault(x => x.BettypeId == (int)EnumBettype.Handicap)?.Value;
                var nova1HHDP = novaMaxbets.FirstOrDefault(x => x.BettypeId == (int)EnumBettype.FirstHalfHandicap)?.Value;

                return BuildMaxbetCN88(cn88Maxbets.ToList(), novaFTHDP, nova1HHDP);
            }

            private async Task<IEnumerable<MaxbetModel>> GetCN88Maxbet(int matchId)
                => await dynamicRepository.FetchAsync<MaxbetModel>(new MaxbetGetCriteria { OWPMatchId = matchId, IsCN88 = true });

            private async Task<IEnumerable<MaxbetModel>> GetNovaMaxbet(int matchId)
                => await dynamicRepository.FetchAsync<MaxbetModel>(new MaxbetGetCriteria { OWPMatchId = matchId, IsCN88 = false });

            private static MaxbetSpread BuildMaxbetCN88(List<MaxbetModel> cn88Maxbets, decimal? novaFTHDP, decimal? nova1HHDP)
            {
                decimal? firstHalfOverUnder = MaxbetHelper.FindMaxbetByBettype(cn88Maxbets, (int)EnumBettype.FirstHalfOverUnder)?.Value
                                    ?? GetMaxbetFromNova(EnumBettype.FirstHalfOverUnder, novaFTHDP, nova1HHDP);

                decimal? firstHalfHandicap = MaxbetHelper.FindMaxbetByBettype(cn88Maxbets, (int)EnumBettype.FirstHalfHandicap)?.Value
                                    ?? GetMaxbetFromNova(EnumBettype.FirstHalfHandicap, novaFTHDP, nova1HHDP);

                var result = new MaxbetSpread
                {
                    Handicap = MaxbetHelper.FindMaxbetByBettype(cn88Maxbets, (int)EnumBettype.Handicap)?.Value ?? GetMaxbetFromNova(EnumBettype.Handicap, novaFTHDP, nova1HHDP),
                    OverUnder = MaxbetHelper.FindMaxbetByBettype(cn88Maxbets, (int)EnumBettype.OverUnder)?.Value ?? GetMaxbetFromNova(EnumBettype.OverUnder, novaFTHDP, nova1HHDP),
                    FirstHalfHandicap = firstHalfHandicap,
                    FirstHalfOverUnder = firstHalfOverUnder
                };

                return result;
            }

            private static decimal? GetMaxbetFromNova(EnumBettype bettype, decimal? novaFTHDP, decimal? nova1HHDP)
            {
                if (!cn88Dict.TryGetValue(Convert.ToInt32(novaFTHDP), out IList<MaxbetModel> maxbets))
                {
                    return bettype switch
                    {
                        EnumBettype.Handicap or EnumBettype.OverUnder => novaFTHDP,
                        _ => nova1HHDP,
                    };
                }

                var isUsingFTHDP = bettype <= EnumBettype.OverUnder;
                var maxbet = maxbets.FirstOrDefault(x => x.BettypeId == (isUsingFTHDP ? (int)EnumBettype.Handicap : (int)EnumBettype.FirstHalfHandicap));
                return maxbet.Value;
            }
        }
    }
}