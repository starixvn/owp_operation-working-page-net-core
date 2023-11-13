using MediatR;
using OWP.EventPicking.Models;
using OWP.EventPicking.Services;

namespace OWP.EventPicking.Handlers;

public class GetLogRequest : IRequest<IList<LogModel>>
{
    public int InfoSiteMatchId { get; set; }
}

public class GetLogHandler : IRequestHandler<GetLogRequest, IList<LogModel>>
{
    private readonly LogService logService;

    public GetLogHandler(LogService logService)
    {
        this.logService = logService;
    }

    public async Task<IList<LogModel>> Handle(GetLogRequest request, CancellationToken cancellationToken)
    => await logService.GetLog(request.InfoSiteMatchId);
}