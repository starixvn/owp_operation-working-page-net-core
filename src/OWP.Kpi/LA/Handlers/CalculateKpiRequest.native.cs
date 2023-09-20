using AutoMapper;
using MediatR;
using OWP.Kpi.LA.Helper;
using OWP.Kpi.LA.Models;
using OWP.Kpi.LA.Services;
using OWP.Kpi.LA.ViewModels;

namespace OWP.Kpi.LA.Handlers;

public class CalculateKpiRequest : IRequest<DataViewModel>
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }
}

public class CalculateKpiHandler : IRequestHandler<CalculateKpiRequest, DataViewModel>
{
    private readonly Repository repository;

    private readonly IMapper mapper;

    public CalculateKpiHandler(Repository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<DataViewModel> Handle(CalculateKpiRequest request, CancellationToken cancellationToken)
    {
        var kpiInfo = await repository.GetKpiInfo(request.FromDate, request.ToDate);

        return new DataViewModel
        {
            KpiLa = CalculateKpi(kpiInfo),
            IsGap = CalculateGapHelper.IsGap(request.FromDate, request.ToDate)
        };
    }

    private IList<KpiInfoViewModel> CalculateKpi(IList<KpiInfo> kpiInfo)
    => mapper.Map<IList<KpiInfoViewModel>>(kpiInfo);
}
