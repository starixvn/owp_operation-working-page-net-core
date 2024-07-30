using MediatR;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers;

public class GetFunctionFeedbackRequest : IRequest<IEnumerable<FunctionFeedback>>;

public class GetFunctionFeedbackHandler : IRequestHandler<GetFunctionFeedbackRequest, IEnumerable<FunctionFeedback>>
{
    private readonly Repository repository;

    public GetFunctionFeedbackHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<FunctionFeedback>> Handle(GetFunctionFeedbackRequest request, CancellationToken cancellationToken)
    {
        return await repository.GetFunctionFeedback();
    }
}
