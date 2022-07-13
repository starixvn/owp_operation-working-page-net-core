using MediatR;
using OWP.LeagueManagement.Models;
using OWP.LeagueManagement.Service;


namespace OWP.LeagueManagement.Handlers
{
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
        public Task<IList<LogDisplayModel>> Handle(ViewLogRequest request, CancellationToken cancellationToken)
            => specialProductService.BuildLog(request.Model);

    }
}