using Fanex.NAP.Client.Helpers.Web;
using OWP.CQRS;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers;

public record ConsolidationGetSubtypeRequest() : Query<IList<ConsolidationSubtypeViewModel>>;

public class ConsolidationGetSubtypeHandler(Repository repository, INapSession session) : QueryHandler<ConsolidationGetSubtypeRequest, IList<ConsolidationSubtypeViewModel>>
{
    private readonly Repository repository= repository;
    public async Task<IList<ConsolidationSubtypeViewModel>> Handle(ConsolidationGetSubtypeRequest request,
        CancellationToken cancellationToken)
    {
        var subtype= await repository.GetSubtype(new ConsolidationGetSubtypeCriteria()
        {
            ActorId = session.UserId
        });

        return subtype.GroupBy(s => s.TypeId)
            .ToDictionary(s => s.Key, s => s.ToList())
            .SelectMany(p =>
            {
                return p.Value.OrderByDescending(v => v.SubTypeId)
                    .Select(v=>new ConsolidationSubtypeViewModel()
                    {
                        TypeId = v.TypeId,
                        SubTypeId = v.SubTypeId,
                        Label = v.SubTypeName
                    });
               
            }).ToList();
    }
}
