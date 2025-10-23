using Fanex.NAP.Client;
using OWP.CQRS;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;

namespace OWP.EventPicking.Api.Handlers;

public record AddSpecialProductRequest(AddSpecialProduct AddInfo) : Command<Unit>;

public class AddSpecialProductHandler(
    INapSession session,
    Repository repository,
    BuildChildMatchMaxBet childMatchMaxBet) : CommandHandler<AddSpecialProductRequest, Unit>
{
    public async Task<Unit> Handle(AddSpecialProductRequest request, CancellationToken cancellationToken)
    {
        var specialProducts = GenerateSpecialProducts(request.AddInfo.ParentId, request.AddInfo.MinorBettypeId);

        if(specialProducts.Count == 0)
        {
            return Unit.Value;
        }

        var mainEvent = await repository.QueryMainMatch(request.AddInfo.ParentId);

        await childMatchMaxBet.BuildMBForAddSpecialProduct(specialProducts, mainEvent);
        await repository.ExecuteNonQuery(new AddSpecialProductCriteria
        {
            ActorId = session.UserId,
            ChildMatches =  XmlConverter.Serialize(specialProducts),
        });
        return Unit.Value;
    }

    private static List<SpecialProductMaxbetSpread> GenerateSpecialProducts(string parentMatchIds, string minorBettypeIds)
    {
        var parentIds = parentMatchIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => int.Parse(x.Trim()))
            .Distinct()
            .ToList();

        var minorIds = minorBettypeIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => int.Parse(x.Trim()))
            .Distinct()
            .ToList();

        var specialProducts = new List<SpecialProductMaxbetSpread>();

        foreach (var parentId in parentIds)
        {
            foreach (var minorId in minorIds)
            {
                specialProducts.Add(new SpecialProductMaxbetSpread
                {
                    ParentId = parentId,
                    MinorBettypeId = minorId,
                    MaxbetHdp = null,
                    MaxbetOu = null,
                    SpreadHdp = null,
                    SpreadOu = null,
                    NoOfHDP = null
                });
            }
        }

        return specialProducts;
    }
}