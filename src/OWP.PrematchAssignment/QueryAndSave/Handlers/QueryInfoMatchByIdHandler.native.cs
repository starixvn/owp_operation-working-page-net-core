using MediatR;
using OWP.PrematchAssignment.QueryAndSave.Models;
using OWP.PrematchAssignment.QueryAndSave.Services;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

public class QueryInfoMatchRequest : IRequest<InfoMatchQueried>
{
    public int Id { get; set; }
}

public class QueryInfoMatchByIdHandler : IRequestHandler<QueryInfoMatchRequest, InfoMatchQueried>
{
    private readonly PreMatchAssignmentService preMatchAssignmentService;

    public QueryInfoMatchByIdHandler(PreMatchAssignmentService preMatchAssignmentService)
    {
        this.preMatchAssignmentService = preMatchAssignmentService;
    }

    public async Task<InfoMatchQueried> Handle(QueryInfoMatchRequest request, CancellationToken cancellationToken)
    {
        var result = await preMatchAssignmentService.GetInfoMatchById(request.Id);

        return result;
    }
}