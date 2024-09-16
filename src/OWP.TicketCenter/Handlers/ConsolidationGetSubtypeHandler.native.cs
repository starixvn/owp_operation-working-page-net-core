using Fanex.NAP.Client.Helpers.Web;
using OWP.CQRS;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers;

public record ConsolidationGetSubtypeRequest : Query<IList<ConsolidationSubtypeModel>>;

public class ConsolidationGetSubtypeHandler(Repository repository, INapSession session) : QueryHandler<ConsolidationGetSubtypeRequest, IList<ConsolidationSubtypeModel>>
{
    public async Task<IList<ConsolidationSubtypeModel>> Handle(ConsolidationGetSubtypeRequest request,
        CancellationToken cancellationToken)
    {
        var consolidation = await repository.GetSubtype(new ConsolidationGetSubtypeCriteria()
        {
            ActorId = session.UserId
        });

        return consolidation.GroupBy(c => new
        {
            c.TypeId,
            c.SubTypeId,
            c.SubTypeName,
            c.EventType
        }).ToDictionary(x => x.Key, x => x.ToList())
        .Select(x => new ConsolidationSubtypeModel
        {
            TypeId = x.Key.TypeId,
            SubTypeId = x.Key.SubTypeId,
            SubTypeName = x.Key.SubTypeName,
            EventType = x.Key.EventType,
            IsRead = !x.Value.Exists(v => !v.IsRead)
        })?.ToList();

    }
}
