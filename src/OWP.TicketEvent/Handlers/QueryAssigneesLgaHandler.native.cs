using Fanex.Data.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.TicketEvent.Handlers;

using Models;

public class QueryAssigneesLgaRequest: IRequest<IEnumerable<TicketAssignees>>
{
    public int OwpMatchId { get; set; }
}

public class QueryAssigneesLgaHandler : IRequestHandler<QueryAssigneesLgaRequest, IEnumerable<TicketAssignees>>
{
    private readonly Repository repository;

    public QueryAssigneesLgaHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<TicketAssignees>> Handle(QueryAssigneesLgaRequest request, CancellationToken cancellationToken)
    {
        var result = await repository.QueryAssigneesByLeagueId(request.OwpMatchId);

        return result;
    }
}