using Fanex.Data.Repository;
using MediatR;
using OWP.AssigneeSuggestionList.Criteria;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Handler;

public class AssignmentRequest : IRequest<Unit>
{
    public DateTime EventDate { get; set; }
}

public class AssignmentHandler : IRequestHandler<AssignmentRequest, Unit>
{
    private readonly IDynamicRepository repository;

    private readonly IMediator mediator;

    public AssignmentHandler(IDynamicRepository repository, IMediator mediator)
    {
        this.repository = repository;
        this.mediator = mediator;
    }

    public async Task<Unit> Handle(AssignmentRequest request, CancellationToken cancellationToken)
    {
        var (assignmentModel, maxbetModel) = await repository.FetchMultipleAsync<MaxbetModel, AssignmentModel>(new QueryAssignmentCriteria
        {
            EventDate = request.EventDate
        });


        return Unit.Value;
    }
}
