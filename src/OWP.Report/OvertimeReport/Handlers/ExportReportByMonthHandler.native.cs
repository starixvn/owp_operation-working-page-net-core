using AutoMapper;
using MediatR;
using OfficeOpenXml;
using OWP.Report.OvertimeReport.Models;
using OWP.Report.OvertimeReport.Services;

namespace OWP.Report.OvertimeReport.Handlers;

public class ExportReportByMonthRequest : IRequest<ExcelPackage>
{
    public string Location { get; set; } = string.Empty;

    public string Trader { get; set; } = string.Empty;

    public int Month { get; set; }

    public int Year { get; set; }
}

public class ExportReportByMonthHandler : IRequestHandler<ExportReportByMonthRequest, ExcelPackage>
{
    private readonly IMapper mapper;
    private readonly ExportReportByMonthServices reportService;

    public ExportReportByMonthHandler(
        IMapper mapper,
        ExportReportByMonthServices reportService)
    {
        this.mapper = mapper;
        this.reportService = reportService;
    }

    public async Task<ExcelPackage> Handle(ExportReportByMonthRequest request, CancellationToken cancellationToken)
    {
        var exportRequest = mapper.Map<OverTimeRequest>(request);

        return await reportService.ExportReportByMonth(exportRequest);
    }
}