using Fanex.NAP.Client.Helpers.Web;
using OWP.CQRS;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers;

public record ConsolidationGetSubtypeRequest() : Query<IList<ConsolidationSubtypeModel>>;

public class ConsolidationGetSubtypeHandler(Repository repository, INapSession session) : QueryHandler<ConsolidationGetSubtypeRequest, IList<ConsolidationSubtypeModel>>
{
    private readonly Repository repository= repository;
    public async Task<IList<ConsolidationSubtypeModel>> Handle(ConsolidationGetSubtypeRequest request,
        CancellationToken cancellationToken)
    {
        return (await repository.GetSubtype(new ConsolidationGetSubtypeCriteria()
        {
            ActorId = session.UserId
        })).ToList();

        
    }
}
