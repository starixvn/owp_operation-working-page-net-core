namespace OWP.OSLeagueSetting.Handlers;

using MediatR;
using Models;
using Services;

public class GetLogSettingRequest : IRequest<IList<LogDisplayModel>>
{
    public int LeagueSettingId { get; set; }
}

public class Handler : IRequestHandler<GetLogSettingRequest, IList<LogDisplayModel>>
{
    private readonly LogService logService;

    public Handler(LogService logService) => this.logService = logService;

    public async Task<IList<LogDisplayModel>> Handle(GetLogSettingRequest request, CancellationToken cancellationToken)
        => await logService.Build(request.LeagueSettingId);
}