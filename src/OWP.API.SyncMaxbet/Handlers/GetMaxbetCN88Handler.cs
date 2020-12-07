namespace OWP.API.SyncMaxbet.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using MediatR;
    using OWP.API.SyncMaxbet.Criterias;
    using OWP.API.SyncMaxbet.Models;
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
            private static readonly ReadOnlyDictionary<int, IList<Maxbet>> cn88Dict = new ReadOnlyDictionary<int, IList<Maxbet>>(
                new Dictionary<int, IList<Maxbet>>
                {
                    { 400, new  List<Maxbet> {
                        new Maxbet { BettypeId = (int)EnumBettype.Handicap, Value = 80},
                        new Maxbet { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 60}
                        }
                    },
                    { 350, new  List<Maxbet> {
                        new Maxbet { BettypeId = (int)EnumBettype.Handicap, Value = 180},
                        new Maxbet { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 40}
                        }
                    },
                    { 200, new  List<Maxbet> {
                        new Maxbet { BettypeId = (int)EnumBettype.Handicap, Value = 150},
                        new Maxbet { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 40}
                        }
                    },
                    { 180, new  List<Maxbet> {
                        new Maxbet { BettypeId = (int)EnumBettype.Handicap, Value = 130},
                        new Maxbet { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 40}
                        }
                    },
                    { 150, new  List<Maxbet> {
                        new Maxbet { BettypeId = (int)EnumBettype.Handicap, Value = 120},
                        new Maxbet { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 40}
                        }
                    },
                    { 130, new  List<Maxbet> {
                        new Maxbet { BettypeId = (int)EnumBettype.Handicap, Value = 110},
                        new Maxbet { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 40}
                        }
                    },
                    { 120, new  List<Maxbet> {
                        new Maxbet { BettypeId = (int)EnumBettype.Handicap, Value = 100},
                        new Maxbet { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 40}
                        }
                    },
                    { 110, new  List<Maxbet> {
                        new Maxbet { BettypeId = (int)EnumBettype.Handicap, Value = 90},
                        new Maxbet { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 40}
                        }
                    },
                    { 100, new  List<Maxbet> {
                        new Maxbet { BettypeId = (int)EnumBettype.Handicap, Value = 80},
                        new Maxbet { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 30}
                        }
                    },
                    { 90, new  List<Maxbet> {
                        new Maxbet { BettypeId = (int)EnumBettype.Handicap, Value = 70},
                        new Maxbet { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 30}
                        }
                    },
                    { 80, new  List<Maxbet> {
                        new Maxbet { BettypeId = (int)EnumBettype.Handicap, Value = 60},
                        new Maxbet { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 24}
                        }
                    },
                    { 70, new  List<Maxbet> {
                        new Maxbet { BettypeId = (int)EnumBettype.Handicap, Value = 55},
                        new Maxbet { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 18}
                        }
                    },
                    { 60, new  List<Maxbet> {
                        new Maxbet { BettypeId = (int)EnumBettype.Handicap, Value = 50},
                        new Maxbet { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 18}
                        }
                    },
                    { 50, new  List<Maxbet> {
                        new Maxbet { BettypeId = (int)EnumBettype.Handicap, Value = 40},
                        new Maxbet { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 18}
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

                return BuildMaxbetCN88(cn88Maxbets.ToList(), novaFTHDP);
            }

            private async Task<IEnumerable<Maxbet>> GetCN88Maxbet(int matchId)
                => await dynamicRepository.FetchAsync<Maxbet>(new MaxbetGetCriteria { OWPMatchId = matchId, IsCN88 = true });

            private async Task<IEnumerable<Maxbet>> GetNovaMaxbet(int matchId)
                => await dynamicRepository.FetchAsync<Maxbet>(new MaxbetGetCriteria { OWPMatchId = matchId, IsCN88 = false });

            private static MaxbetSpread BuildMaxbetCN88(List<Maxbet> cn88Maxbets, decimal? novaFTHDP)
            {
                var result = new MaxbetSpread();
                result.Handicap = MaxbetHelper.FindMaxbetByBettype(cn88Maxbets, (int)EnumBettype.Handicap)?.Value ?? GetMaxbetFromNova(EnumBettype.Handicap, novaFTHDP);
                result.OverUnder = MaxbetHelper.FindMaxbetByBettype(cn88Maxbets, (int)EnumBettype.OverUnder)?.Value ?? GetMaxbetFromNova(EnumBettype.OverUnder, novaFTHDP);
                result.FirstHalfHandicap = MaxbetHelper.FindMaxbetByBettype(cn88Maxbets, (int)EnumBettype.FirstHalfHandicap)?.Value ?? GetMaxbetFromNova(EnumBettype.FirstHalfHandicap, novaFTHDP);
                result.FirstHalfOverUnder = MaxbetHelper.FindMaxbetByBettype(cn88Maxbets, (int)EnumBettype.FirstHalfOverUnder)?.Value ?? GetMaxbetFromNova(EnumBettype.FirstHalfOverUnder, novaFTHDP);

                return result;
            }

            private static decimal GetMaxbetFromNova(EnumBettype bettype, decimal? novaFTHDP)
            {
                if (!cn88Dict.TryGetValue(Convert.ToInt32(novaFTHDP), out IList<Maxbet> maxbets))
                {
                    return novaFTHDP ?? 0;
                }

                var isUsingFTHDP = bettype <= EnumBettype.OverUnder;
                var maxbet = maxbets.FirstOrDefault(x => x.BettypeId == (isUsingFTHDP ? (int)EnumBettype.Handicap : (int)EnumBettype.FirstHalfHandicap));
                return maxbet.Value;
            }
        }
    }
}