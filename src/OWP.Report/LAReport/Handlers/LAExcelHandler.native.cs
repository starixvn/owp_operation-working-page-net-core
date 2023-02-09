namespace OWP.Report.LAReport.Handlers;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Fanex.NAP.Client.Helpers.Web;
using MediatR;
using Models;
using OfficeOpenXml;
using OWP.Services;
using Report.Models;
using Services;

public class LAExcelRequest : IRequest<ExcelPackage>
{
    public string FromDate { get; set; }

    public string ToDate { get; set; }

    public IEnumerable<LAReportModel> ReportModels { get; set; }
}

public class LAExcelHandler : IRequestHandler<LAExcelRequest, ExcelPackage>
{
    private readonly IMapper mapper;

    private readonly ExportExcelService exportExcelService;

    private readonly INapSession currentSession;

    private readonly ConfigurationService configurationService;

    public LAExcelHandler(
        IMapper mapper,
        ConfigurationService configurationService,
        ExportExcelService exportExcelService,
        INapSession currentSession)
    {
        this.mapper = mapper;
        this.configurationService = configurationService;
        this.exportExcelService = exportExcelService;
        this.currentSession = currentSession;
    }

    public async Task<ExcelPackage> Handle(LAExcelRequest request, CancellationToken cancellationToken)
    {
        var templatePath = @$"App_Data{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}LA-Report.xlsx";
        var absoluteTemplatePath = $"{configurationService.CurrentDirectory}{Path.DirectorySeparatorChar}{templatePath}";

        var exportExcelModels = mapper.Map<List<LAExcelReportModel>>(request.ReportModels);
        var utcTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
        var exportTime = utcTime.AddHours(8);
        var infoExportFile = new ExcelFileInfo
        {
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            ExportBy = currentSession.UserName,
            ExportTime = exportTime.ToString("MM/dd/yyyy HH:mm:ss")
        };
        var exportExcelModel = new LAExcelModel
        {
            ExcelModels = exportExcelModels,
            FileInfo = infoExportFile
        };


        var excelPackage = exportExcelService.CreateExcelPackage(absoluteTemplatePath, exportExcelModel);

        return await Task.FromResult(excelPackage);
    }
}