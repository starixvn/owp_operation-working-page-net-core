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
    using OWP.Enums;
    using OWP.Helpers;
    using OWP.Maxbet.Criteria;
    using OWP.Maxbet.Models;

    public class GetMaxbetCN88Handler
    {
        public class Request : IRequest<MaxbetSpread>
        {
            public int OWPMatchId { get; set; }
        }

        public class Handler : IRequestHandler<Request, MaxbetSpread>
        {
            private static readonly ReadOnlyDictionary<int, IList<MaxbetModel>> cn88GreaterThan40KDict = new ReadOnlyDictionary<int, IList<MaxbetModel>>(
                new Dictionary<int, IList<MaxbetModel>>
                {
                    { 400000, CreateMaxbets(200000, 60000) },
                    { 350000, CreateMaxbets(180000, 40000) },
                    { 200000, CreateMaxbets(150000, 40000) },
                    { 180000, CreateMaxbets(130000, 40000) },
                    { 150000, CreateMaxbets(120000, 40000) },
                    { 130000, CreateMaxbets(110000, 40000) },
                    { 120000, CreateMaxbets(100000, 40000) },
                    { 110000, CreateMaxbets(90000, 40000) },
                    { 100000, CreateMaxbets(80000, 30000) },
                    { 90000, CreateMaxbets(70000, 30000) },
                    { 80000, CreateMaxbets(60000, 24000) },
                    { 70000, CreateMaxbets(55000, 18000) },
                    { 60000, CreateMaxbets(50000, 18000) },
                    { 50000, CreateMaxbets(40000, 18000) },
                }
            );

            private static readonly ReadOnlyDictionary<int, int> cn88LowerThan40KDict = new ReadOnlyDictionary<int, int>(
                new Dictionary<int, int>
                {
                    { 40000, 25000 },
                    { 35000, 20000 },
                    { 30000, 18000 },
                    { 25000, 15000 },
                    { 20000, 12000 },
                    { 18000, 10000 },
                    { 15000, 10000 },
                    { 12000, 8000 },
                    { 10000, 6000 },
                    { 8000, 5000 },
                    { 7000, 4000 },
                    { 6000, 4000 },
                    { 5000, 3000 },
                    { 4000, 2000 },
                    { 3000, 2000 },
                    { 2000, 2000 }
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

            private async Task<IEnumerable<MaxbetModel>> GetCN88Maxbet(int matchId)
                => await dynamicRepository.FetchAsync<MaxbetModel>(new MaxbetGetCriteria { OWPMatchId = matchId, IsCN88 = true });

            private async Task<IEnumerable<MaxbetModel>> GetNovaMaxbet(int matchId)
                => await dynamicRepository.FetchAsync<MaxbetModel>(new MaxbetGetCriteria { OWPMatchId = matchId, IsCN88 = false });

            private static MaxbetSpread BuildMaxbetCN88(List<MaxbetModel> cn88Maxbets, decimal? novaFTHDP)
            {
                decimal? firstHalfOverUnder = MaxbetHelper.FindMaxbetByBettype(cn88Maxbets, (int)EnumBettype.FirstHalfOverUnder)?.Value
                                    ?? Get1HMaxbetFromNova(novaFTHDP);

                decimal? firstHalfHandicap = MaxbetHelper.FindMaxbetByBettype(cn88Maxbets, (int)EnumBettype.FirstHalfHandicap)?.Value
                                    ?? Get1HMaxbetFromNova(novaFTHDP);

                var result = new MaxbetSpread
                {
                    Handicap = MaxbetHelper.FindMaxbetByBettype(cn88Maxbets, (int)EnumBettype.Handicap)?.Value ?? GetFTMaxbetFromNova(novaFTHDP),
                    OverUnder = MaxbetHelper.FindMaxbetByBettype(cn88Maxbets, (int)EnumBettype.OverUnder)?.Value ?? GetFTMaxbetFromNova(novaFTHDP),
                    FirstHalfHandicap = firstHalfHandicap,
                    FirstHalfOverUnder = firstHalfOverUnder
                };

                return result;
            }

            private static decimal? GetFTMaxbetFromNova(decimal? novaFTHDP)
            {
                if (!cn88GreaterThan40KDict.TryGetValue(Convert.ToInt32(novaFTHDP), out IList<MaxbetModel> maxbets))
                {
                    return novaFTHDP;
                }

                var maxbet = maxbets.FirstOrDefault(x => x.BettypeId == (int)EnumBettype.Handicap);
                return maxbet.Value;
            }

            private static decimal? Get1HMaxbetFromNova(decimal? novaFTHDP)
            {
                if (!novaFTHDP.HasValue)
                {
                    return null;
                }

                const int flagValue = 50000;

                if (novaFTHDP.Value >= flagValue)
                {
                    return GetMaxbetValueLargerThan40K(novaFTHDP);
                }

                return GetMaxbetValueLowerThan40K(novaFTHDP);
            }

            private static decimal? GetMaxbetValueLargerThan40K(decimal? novaFTHDP)
            {
                if (!cn88GreaterThan40KDict.TryGetValue(Convert.ToInt32(novaFTHDP), out IList<MaxbetModel> maxbets))
                {
                    return novaFTHDP;
                }

                var maxbet = maxbets.FirstOrDefault(x => x.BettypeId == (int)EnumBettype.FirstHalfHandicap);
                return maxbet.Value;
            }

            private static decimal? GetMaxbetValueLowerThan40K(decimal? novaFTHDP)
            {
                if (!cn88LowerThan40KDict.TryGetValue(Convert.ToInt32(novaFTHDP), out int nova1H))
                {
                    return novaFTHDP;
                }

                return nova1H;
            }

            private static IList<MaxbetModel> CreateMaxbets(decimal hdp, decimal firstHalfHDP)
                => new List<MaxbetModel> {
                        new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = hdp},
                        new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = firstHalfHDP}
                        };
        }
    }
}