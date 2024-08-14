using Fanex.NAP.Client.Helpers.Web;
using OWP.CQRS;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers;

public record ConsolidationGetSubtypeRequest() : Query<IList<ConsolidationSubtypeViewModel>>;

public class ConsolidationGetSubtypeHandler(Repository repository, INapSession session) : QueryHandler<ConsolidationGetSubtypeRequest, IList<ConsolidationSubtypeViewModel>>
{
    public async Task<IList<ConsolidationSubtypeViewModel>> Handle(ConsolidationGetSubtypeRequest request,
        CancellationToken cancellationToken)
    {
        var subtype= await repository.GetSubtype(new ConsolidationGetSubtypeCriteria()
        {
            ActorId = session.UserId
        });

        return subtype.GroupBy(s => new{s.TypeId, s.TypeName})
            .ToDictionary(s => s.Key, s => s.ToList())
            .Select(s=>new ConsolidationSubtypeViewModel
            {
                Id = s.Key.TypeId,
                Label = s.Key.TypeName,
                ChildNode = s.Value
                    .DistinctBy(c=>c.SubTypeId)
                    .OrderBy(c => c.SubTypeId)
                    .Select(c=>new ConsolidationSubtypeViewModel()
                {
                    Id = c.SubTypeId,
                    Label = c.SubTypeName,
                }).ToList()
            }).ToList();
    }
}
