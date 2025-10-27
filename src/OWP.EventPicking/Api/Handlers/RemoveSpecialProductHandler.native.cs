using Fanex.NAP.Client;
using OWP.EventPicking.Api.Helper;

namespace OWP.EventPicking.Api.Handlers;

public record RemoveSpecialProductRequest(RemoveSpecialProduct RemoveInfo) : Command<Unit>;

public class RemoveSpecialProductHandler(INapSession session, Repository repository) : CommandHandler<RemoveSpecialProductRequest, Unit>
{
    public async Task<Unit> Handle(RemoveSpecialProductRequest request, CancellationToken cancellationToken)
    {
        await repository.ExecuteNonQuery(new RemoveSpecialProductCriteria
        {
            ActorId = session.UserId,
            ParentId = request.RemoveInfo.ParentId,
            MinorBettypeId = request.RemoveInfo.MinorBettypeId,
        });

        var specialProductIds = request.RemoveInfo.MinorBettypeId
            .Split(',')
            .Select(int.Parse)
            .ToList();

        var parentIds = request.RemoveInfo.ParentId
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => int.Parse(x.Trim()))
            .ToList();

        // Parse existing MinorBettypeIds mapping (JSON format: {"parentId": "1,2,3"})
        var existingMinorBettypeIdsMap = SpecialProductHelper.ParseExistingMinorBettypeIds(request.RemoveInfo.ExistingMinorBettypeIds);

        // Write log only for existing MinorBettypeIds that are actually removed
        await SpecialProductHelper.WriteLogForRemoveSpecialProduct(
            repository,
            specialProductIds,
            parentIds,
            existingMinorBettypeIdsMap,
            session.UserName,
            cancellationToken);

        return Unit.Value;
    }
}