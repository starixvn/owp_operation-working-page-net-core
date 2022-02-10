using OWP.PreMatchReport.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OWP.PreMatchReport.Models;
using OWP.PreMatchReport.Services;
using MediatR;
namespace OWP.PreMatchReport.Handlers;
public class ReportRequest : QueryModel, IRequest<IEnumerable<ReportModel>>
{
}

public class ReportHandler : IRequestHandler<ReportRequest, IEnumerable<ReportModel>>
{
    private readonly GetPreMatchReportService service;

    public ReportHandler(GetPreMatchReportService service)
    {
        this.service = service;
    }

    public async Task<IEnumerable<ReportModel>> Handle(ReportRequest request, CancellationToken cancellationToken)
        => await service.GetReports(request);
}
