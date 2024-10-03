using Fanex.NAP.Client;
using OWP.CQRS;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api.Handlers;

public record AddSpecialProductRequest(AddSpecialProduct AddInfo) : Command<Unit>;

public class AddSpecialProductHandler(INapSession session, Repository repository) : CommandHandler<AddSpecialProductRequest, Unit>
{
    public async Task<Unit> Handle(AddSpecialProductRequest request, CancellationToken cancellationToken)
    {
        await repository.ExecuteNonQuery(new AddSpecialProductCriteria
        {
            ActorId = session.UserId,
            ParentId = request.AddInfo.ParentId,
            MinorBettypeId = request.AddInfo.MinorBettypeId,
        });
        return Unit.Value;
    }
}