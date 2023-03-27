using Fanex.Data.Repository;
using MediatR;
using OWP.AssigneeSuggestionList.Criteria;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services;

public interface AssignmentService
{
    Task<(IList<MaxbetModel>, IList<AssignmentModel>)> QueryAssignments(DateTime eventDate);
}

public class AssignmentServiceImp : AssignmentService
{
    private readonly IDynamicRepository repository;

    public AssignmentServiceImp(IDynamicRepository repository, IMediator mediator)
    {
        this.repository = repository;
    }


    public async Task<(IList<MaxbetModel>, IList<AssignmentModel>)> QueryAssignments(DateTime eventDate)
    {
        var (assignmentModel, maxbetModel) = await repository.FetchMultipleAsync<MaxbetModel, AssignmentModel>(new QueryAssignmentCriteria
        {
            EventDate = eventDate
        });
        return (assignmentModel.ToList(), maxbetModel.ToList());
    }
}
