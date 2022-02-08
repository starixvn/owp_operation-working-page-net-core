using MediatR;
using OWP.PreMatchAssignment.Services;

namespace OWP.PrematchAssignment.Handlers;

public class ExportPreMatchAssignmentRequest : IRequest<byte[]>
{
}

public class ExportPreMatchAssignmentHandler : IRequestHandler<ExportPreMatchAssignmentRequest, byte[]>
{
    private readonly PreMatchReportExportService reportExportService;

    public ExportPreMatchAssignmentHandler(PreMatchReportExportService reportExportService)
    {
        this.reportExportService = reportExportService;
    }

    public async Task<byte[]> Handle(ExportPreMatchAssignmentRequest request, CancellationToken cancellationToken)
    {
        return await reportExportService.Export();
    }
}