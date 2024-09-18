using Fanex.Data.Repository;
using MediatR;
using Newtonsoft.Json;
using Olympus.OWP.Constants;
using OWP.Api.Trader.Criteria;
using OWP.Api.Trader.Model;

namespace OWP.Api.Trader.Handlers;

public class TraderRequest : IRequest<IEnumerable<TraderMatch>>
{
    public List<MatchRequest> matches { get; set; }
}

public class GetTraderHandler : IRequestHandler<TraderRequest, IEnumerable<TraderMatch>>
{
    private readonly IDynamicRepository dynamicRepository;

    public GetTraderHandler(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<IEnumerable<TraderMatch>> Handle(TraderRequest request, CancellationToken cancellationToken)
    {
        var owpMatchRequest = MapSubtypeToBettype(request.matches);
        var owpAssignments = await GetTradersByAdminsiteMatchIds(owpMatchRequest.ToList());

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

    public List<OWPMatchRequest> MapSubtypeToBettype(List<MatchRequest> matches)
        => matches.Select(match => new OWPMatchRequest
        {
            MatchId = match.MatchId,
            ParentId = match.ParentId,
            Subtype = match.Subtype,
            MinorBettypeId = GetMinorBettypeIdBySubtype(match.Subtype)
        }).ToList();

    private static int? GetMinorBettypeIdBySubtype(int subtype)
        => subtype == 0 ? null : MinorBettype.GetMinorBettypeIdBySubtype(subtype);

    public async Task<List<OWPAssignment>> GetTradersByAdminsiteMatchIds(IEnumerable<OWPMatchRequest> matches)
    {
        var criteria = new GetTraderByAdminsiteMatchIdsCriteria
        {
            MatchesJson = JsonConvert.SerializeObject(matches)
        };

        return (await dynamicRepository.FetchAsync<OWPAssignment>(criteria)).ToList();
    }
}
