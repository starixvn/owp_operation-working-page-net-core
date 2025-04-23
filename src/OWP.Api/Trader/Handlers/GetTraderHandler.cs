using Fanex.Data.Repository;
using MediatR;
using Newtonsoft.Json;
using Olympus.OWP.Constants;
using OWP.Api.Maxbet.Constants;
using OWP.Api.Maxbet.Helpers;
using OWP.Api.Trader.Criteria;
using OWP.Api.Trader.Model;

namespace OWP.Api.Trader.Handlers;

public class TraderRequest : IRequest<IEnumerable<TraderMatch>>
{
    public List<MatchRequest> Matches { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class GetTraderHandler(IDynamicRepository dynamicRepository)
    : IRequestHandler<TraderRequest, IEnumerable<TraderMatch>>
{
    public async Task<IEnumerable<TraderMatch>> Handle(TraderRequest request, CancellationToken cancellationToken)
    {
        var owpMatchRequest = MapSubtypeToBettype(request.Matches);
        var owpAssignments = await GetTradersByAdminsiteMatchIds(owpMatchRequest.ToList(), request.FromDate, request.ToDate);

        return owpAssignments
            .GroupBy(x => x.MatchId)
            .Select(x => new TraderMatch
            {
                MatchId = x.Key,
                Traders = x.Where(b => b.Bettype != 0 && b.UserId != 0)
                    .Select(a => new AssignedTrader
                    {
                        Bettype = a.Bettype,
                        TraderId = a.UserId
                    }).ToList()
            });
    }

    public IEnumerable<OWPMatchRequest> MapSubtypeToBettype(List<MatchRequest> matches)
    {
        foreach (var match in matches)
        {
            var isETPKOfMainMatch = MaxbetHelper.IsETPKOfMainMatch(match.Subtype);
            if (isETPKOfMainMatch)
            {
                yield return new OWPMatchRequest
                {
                    MatchId = match.MatchId,
                    ParentId = match.ParentId,
                    Subtype = 0,
                    MinorBettypeId = 0,
                    IsMainMatch = true
                };
            }
            else
            {
                yield return new OWPMatchRequest
                {
                    MatchId = match.MatchId,
                    ParentId = match.ParentId == 0 ? match.MatchId : match.ParentId,
                    Subtype = match.Subtype,
                    MinorBettypeId = GetMinorBettypeIdBySubtype(match.Subtype),
                    IsMainMatch = match.Subtype == 0 && match.ParentId == 0
                };
            }
        }
    }

    public static int GetSubtypeSpecial(int subtype)
        => SubTypeETPK.SpecialSubType.GetValueOrDefault(subtype, subtype);

    private static int? GetMinorBettypeIdBySubtype(int subtype)
    {
        var subtypeSpecial = GetSubtypeSpecial(subtype);
        return subtype == 0 ? null : MinorBettype.GetMinorBettypeIdBySubtype(subtypeSpecial);
    }

    public async Task<List<OWPAssignment>> GetTradersByAdminsiteMatchIds(IEnumerable<OWPMatchRequest> matches,DateTime? fromDate, DateTime? toDate)
    {
        var criteria = new GetTraderByAdminsiteMatchIdsCriteria
        {
            MatchesJson = JsonConvert.SerializeObject(matches),
            FromDate = fromDate,
            ToDate = toDate
        };

        return (await dynamicRepository.FetchAsync<OWPAssignment>(criteria)).ToList();
    }
}
