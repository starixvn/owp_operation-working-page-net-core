using OWP.LeagueManagement.Services;

namespace OWP.LeagueManagement.Handlers;

using MediatR;
using Models;

public class ViewLogRequest : IRequest<IList<LogDisplayModel>>
{
    public QueryLogModel Model { get; set; }
}
public class ViewLogHandler : IRequestHandler<ViewLogRequest, IList<LogDisplayModel>>
{
    private readonly SpecialProductService specialProductService;
    public ViewLogHandler(SpecialProductService specialProductService)
    {
        this.specialProductService = specialProductService;
    }
    public async Task<IList<LogDisplayModel>> Handle(ViewLogRequest request, CancellationToken cancellationToken)
        => await specialProductService.BuildLog(request.Model);

}