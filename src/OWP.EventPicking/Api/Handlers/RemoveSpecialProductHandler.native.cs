using OWP.CQRS;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Models;

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
        return Unit.Value;
    }
}