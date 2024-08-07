using OWP.CQRS;
using OWP.EventPicking.Api.Criteria;

namespace OWP.EventPicking.Api.Handlers;

public record RemoveSpecialProductRequest(int[] Id) : Command<Unit>;

public class RemoveSpecialProductHandler(INapSession session, Repository repository) : CommandHandler<RemoveSpecialProductRequest, Unit>
{
    public async Task<Unit> Handle(RemoveSpecialProductRequest request, CancellationToken cancellationToken)
    {
        await repository.ExecuteNonQuery(new RemoveSpecialProductCriteria
        {
            ActorId = session.UserId,
            Id = string.Join(",", request.Id ?? [])
        });
        return Unit.Value;
    }
}