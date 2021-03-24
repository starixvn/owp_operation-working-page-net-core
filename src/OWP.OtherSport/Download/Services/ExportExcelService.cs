namespace OWP.OtherSport.Download.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Epplus.Mapper.Extensions;
    using OfficeOpenXml;
    using OWP.OtherSport.Download.Constants;
    using OWP.OtherSport.Download.Models;
    using OWP.OtherSport.Submit.Models;
    using OWP.Services;

    public interface ExportExcelService
    {
        Task<ExcelPackage> CreateExcelPackage(DownloadAssignmentRequest request);
    }

    public class ExportExcelServiceImp : ExportExcelService
    {
        private readonly ConfigurationService configurationService;

        private readonly BuildExcelModelService buildExcelModelService;

        private ExcelPackage excelPackage { get; set; }

        private ExcelWorksheet reportSheet;

        public ExportExcelServiceImp(
            ConfigurationService configurationService,
            BuildExcelModelService buildExcelModelService)
        {
            this.configurationService = configurationService;
            this.buildExcelModelService = buildExcelModelService;
        }

        public async Task<ExcelPackage> CreateExcelPackage(DownloadAssignmentRequest request)
        {
            const string templatePath = @"App_Data\Templates\ExportOSAssignmentTemplate.xlsx";

            var absolutedTemplatePath = $"D:\\OWP\\operation-working-page-net-core\\OWP.Websites\\{templatePath}";
            //var absolutedTemplatePath = $"{configurationService.CurrentDirectory}\\{templatePath}";
            var model = await buildExcelModelService.BuildExcelModel(request);

            return CreateExcelPackage(absolutedTemplatePath, model);
        }

        public ExcelPackage CreateExcelPackage(string templatePath, ExcelModel model)
        {
            LoadTemplate(templatePath);
            BuildLAReportBody(model);
            ClearTemplate();
            return excelPackage;
        }

        public void LoadTemplate(string templatePath)
        {
            using (var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            {
                excelPackage = new ExcelPackage(stream);
                reportSheet = excelPackage.Workbook.Worksheets.Copy(ExcelConst.TemplateName, ExcelConst.SheetName);
            }
        }

        public void BuildLAReportBody(ExcelModel model)
        {
            reportSheet.ApplyModel(model.HeaderInfo);
            BuildAssignments(model.ExcelAssignmentModels);
        }

        public void ClearTemplate()
        {
            excelPackage.Workbook.Worksheets.Delete(ExcelConst.TemplateName);
        }

        private void BuildAssignments(ExcelAssignmentModel model)
        {
            for (int i = 0, row = ExcelConst.StartRowIndex; i < model.Matches.Count; i++, row++)
            {
                var match = model.Matches[i];
                var assignments = model.MatchAssignments[match.OSMatchId];
                var maxbets = model.MaxBet[match.OSMatchId];

                BuildMatchInfo(row, match);
                BuildTrader(row, assignments);
                BuildMaxbet(row, maxbets);
            }
        }

        private void BuildMatchInfo(int row, MatchInfo assignment)
        {
            reportSheet.Cells[row, MatchInfoConst.MatchNoColIndex].Value = assignment.MatchNo;
            reportSheet.Cells[row, MatchInfoConst.MatchStatusColIndex].Value = assignment.MatchStatus;
            reportSheet.Cells[row, MatchInfoConst.SportColIndex].Value = assignment.Sport;
            reportSheet.Cells[row, MatchInfoConst.DateTimeColIndex].Value = assignment.DateTime;
            reportSheet.Cells[row, MatchInfoConst.LeagueColIndex].Value = assignment.League;
            reportSheet.Cells[row, MatchInfoConst.HomeColIndex].Value = assignment.Home;
            reportSheet.Cells[row, MatchInfoConst.AwayColIndex].Value = assignment.Away;
            reportSheet.Cells[row, MatchInfoConst.CategoryColIndex].Value = assignment.Category;
            reportSheet.Cells[row, MatchInfoConst.NoHDPColIndex].Value = assignment.NoHDP;
            reportSheet.Cells[row, MatchInfoConst.MatchOfferColIndex].Value = assignment.MatchOffer;
            reportSheet.Cells[row, MatchInfoConst.AutoLinkColIndex].Value = assignment.AutoLink;
            reportSheet.Cells[row, MatchInfoConst.ProviderColIndex].Value = assignment.Provider;
            reportSheet.Cells[row, MatchInfoConst.OtherProviderColIndex].Value = assignment.OtherProvider;
            reportSheet.Cells[row, MatchInfoConst.ServiceColIndex].Value = assignment.Service;
            reportSheet.Cells[row, MatchInfoConst.IPTVColIndex].Value = assignment.IPTV;
            reportSheet.Cells[row, MatchInfoConst.WBColIndex].Value = assignment.WB;
            reportSheet.Cells[row, MatchInfoConst.ReporterColIndex].Value = assignment.Reporter;
            reportSheet.Cells[row, MatchInfoConst.RemarkColIndex].Value = assignment.Remark;
        }

        private void BuildTrader(int row, List<ExcelMatchAssignment> assignments)
        {
            foreach (var assignment in assignments)
            {
                reportSheet.Cells[row, assignment.ColumnIndex].Value = assignment.Value;
            }
        }

        private void BuildMaxbet(int row, List<ExcelMatchMaxBet> maxbets)
        {
            foreach (var maxbet in maxbets)
            {
                reportSheet.Cells[row, maxbet.ColumnIndex].Value = maxbet.Value;
            }
        }
    }
}
