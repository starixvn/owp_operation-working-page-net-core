using Fanex.NAP.Client;
using OWP.EventPicking.Api.Helper;
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

        if (specialProducts.Count == 0)
        {
            return Unit.Value;
        }

        var mainEvent = await repository.QueryMainMatch(request.AddInfo.ParentId);

        await childMatchMaxBet.BuildMBForAddSpecialProduct(specialProducts, mainEvent);
        await repository.ExecuteNonQuery(new AddSpecialProductCriteria
        {
            ActorId = session.UserId,
            ChildMatches = XmlConverter.Serialize(specialProducts),
        });

        var specialProductIds = request.AddInfo.MinorBettypeId
            .Split(',')
            .Select(int.Parse)
            .ToList();

        var parentIds = request.AddInfo.ParentId
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => int.Parse(x.Trim()))
            .ToList();

        // Parse existing MinorBettypeIds mapping (JSON format: {"parentId": "1,2,3"})
        var existingMinorBettypeIdsMap = SpecialProductHelper.ParseExistingMinorBettypeIds(request.AddInfo.ExistingMinorBettypeIds);

        // Write log only for new MinorBettypeIds
        await SpecialProductHelper.WriteLogForAddSpecialProduct(
            repository,
            specialProductIds,
            parentIds,
            existingMinorBettypeIdsMap,
            session.UserName,
            cancellationToken);

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