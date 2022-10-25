namespace OWP.Report.TraderReport.Handlers
{
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

    public class ExcelRequest : IRequest<ExcelPackage>
    {
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public IEnumerable<ReportModel> ReportModels { get; set; }
    }

    public class ExcelHandler : IRequestHandler<ExcelRequest, ExcelPackage>
    {
        private readonly IMapper mapper;
        private readonly ConfigurationService configurationService;
        private readonly INapSession currentSession;
        private readonly ReportPermissionService reportPermission;
        private readonly ExportExcelService exportExcelService;
        private readonly ExcelPermissionService excelPermissionService;

        public ExcelHandler(
            IMapper mapper,
            ConfigurationService configurationService,
            INapSession currentSession,
            ReportPermissionService reportPermission,
            ExportExcelService exportExcelService,
            ExcelPermissionService excelPermissionService)
        {
            this.mapper = mapper;
            this.configurationService = configurationService;
            this.currentSession = currentSession;
            this.reportPermission = reportPermission;
            this.exportExcelService = exportExcelService;
            this.excelPermissionService = excelPermissionService;
        }

        public async Task<ExcelPackage> Handle(ExcelRequest request, CancellationToken cancellationToken)
        {
            var templatePath = @$"App_Data{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}LST-Report.xlsx";
            var absoluteTemplatePath = $"{configurationService.CurrentDirectory}{Path.DirectorySeparatorChar}{templatePath}";

            var exportExcelModels = mapper.Map<List<ExcelReportModel>>(request.ReportModels);
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            var exportTime = utcTime.AddHours(8);
            var infoExportFile = new ExcelFileInfo
            {
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                ExportBy = currentSession.UserName,
                ExportTime = exportTime.ToString("MM/dd/yyyy HH:mm:ss")
            };
            var exportExcelModel = new ExcelModel
            {
                ExcelModels = exportExcelModels,
                FileInfo = infoExportFile
            };

            var excelPackage = exportExcelService.CreateExcelPackage(absoluteTemplatePath, exportExcelModel);

            var permission = reportPermission.BuildPermission();
            excelPermissionService.RemoveColumnRelyOnPermission(excelPackage, permission);

            return await Task.FromResult(excelPackage);
        }
    }
}