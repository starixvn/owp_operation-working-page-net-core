using AutoMapper;
using MediatR;
using OWP.Kpi.LA.Models;
using OWP.Kpi.LA.Services;

namespace OWP.Kpi.LA.Handlers;

public class CalculateKpiRequest : IRequest<IList<KpiInfoViewModel>>
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }
}

public class CalculateKpiHandler : IRequestHandler<CalculateKpiRequest, IList<KpiInfoViewModel>>
{
    private readonly KpiService kpiService;

    private readonly IMapper mapper;

    public CalculateKpiHandler(KpiService kpiService, IMapper mapper)
    {
        this.kpiService = kpiService;
        this.mapper = mapper;
    }

    public async Task<IList<KpiInfoViewModel>> Handle(CalculateKpiRequest request, CancellationToken cancellationToken)
    {
        var kpiInfo = await kpiService.GetKpiInfo(request.FromDate, request.ToDate);
        return CalculateKpi(kpiInfo);
    }

    private IList<KpiInfoViewModel> CalculateKpi(IList<KpiInfo> kpiInfo)
    => mapper.Map<IList<KpiInfoViewModel>>(kpiInfo);
}
