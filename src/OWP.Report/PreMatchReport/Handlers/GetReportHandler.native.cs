using MediatR;
using OWP.Report.PreMatchReport.Models;
using OWP.Report.PreMatchReport.Services;

namespace OWP.Report.PreMatchReport.Handlers;

public class ReportRequest : QueryModel, IRequest<List<ExportPreMatchModel>>
{
}

public class GetReportHandler : IRequestHandler<ReportRequest, List<ExportPreMatchModel>>
{
    private readonly GetPreMatchReportService service;

    public GetReportHandler(GetPreMatchReportService service)
    {
        this.service = service;
    }

    public async Task<List<ExportPreMatchModel>> Handle(ReportRequest request, CancellationToken cancellationToken)
        => (await service.GetReports(request)).ToList();
}