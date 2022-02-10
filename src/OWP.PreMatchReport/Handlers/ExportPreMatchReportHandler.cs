using MediatR;
using OWP.PreMatchReport.Services;

namespace OWP.PreMatchReport.Handlers;

public class ExportPreMatchReportRequest : IRequest<byte[]>
{
}

public class ExportPreMatchReportHandler : IRequestHandler<ExportPreMatchReportRequest, byte[]>
{
    private readonly PreMatchReportExportService reportExportService;

    public ExportPreMatchReportHandler(PreMatchReportExportService reportExportService)
    {
        this.reportExportService = reportExportService;
    }

    public async Task<byte[]> Handle(ExportPreMatchReportRequest request, CancellationToken cancellationToken)
    {
        return await reportExportService.Export();
    }
}