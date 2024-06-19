using Fanex.NAP.Client.Helpers.Web;
namespace OWP.OddsCompiler;
using AutoMapper;
using Criteria;
using Epplus.Mapper.Extensions;
using Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public interface ExcelExportService
{
    ExcelPackage CreateExcelPackage(string templatePath, ExcelModel model);

    Task<ExcelModel> GetExcelModel(GetOddsCompilerCriteria criteria);
}

public class ExcelExportServiceImp : ExcelExportService
{
    private readonly Repository oddsCompilerRepository;

    private readonly INapSession currentSession;

    private readonly IMapper mapper;


    private ExcelPackage excelPackage { get; set; }

    private ExcelWorksheet reportSheet;

    private const string TemplateName = "Sheet0";

    private const string SheetName = "Odds";

    private const int StartRowIndex = 3;

    public ExcelExportServiceImp(Repository oddsCompilerRepository,
        INapSession currentSession,
        IMapper mapper
        )
    {
        this.oddsCompilerRepository = oddsCompilerRepository;
        this.currentSession = currentSession;
        this.mapper = mapper;
    }

    public ExcelPackage CreateExcelPackage(string templatePath, ExcelModel model)
    {
        LoadTemplate(templatePath);
        BuildReportBody(model);
        ClearTemplate();
        return excelPackage;
    }

    public async Task<ExcelModel> GetExcelModel(GetOddsCompilerCriteria criteria)
    {
        var matches = await oddsCompilerRepository.GetOddsCompiler(criteria);
        var matchStatuses = await oddsCompilerRepository.GetResourceByName("MatchStatus");
        var filterMatchStatus = criteria.MatchStatusIds.Split(',').Select(int.Parse).ToList();

        var excelModel = new ExcelModel
        {
            FileInfo = new ExcelFileInfo
            {
                Date = criteria.Date.ToString("MMMM dd, yyyy"),
                ExportTime = $"export time: {DateTimeOffset.Now.ToString("MM/dd/yyyy HH:mm:ss")}",
                ExportBy = $"export by {currentSession.UserName}",
                IsExcludeSpecialProducts = $"exclude special products: {criteria.IsExcludeSpecialProducts}",
                League = criteria.League,
                MatchStatus = string.Join(", ",
                    matchStatuses.Where(x => filterMatchStatus.Contains(x.ResourceId)).Select(x => x.Value))
            },
            ExcelModels = mapper.Map<List<ExcelReportModel>>(matches)
        };


        return excelModel;
    }



    private void LoadTemplate(string templatePath)
    {
        using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
        excelPackage = new ExcelPackage(stream);
        reportSheet = excelPackage.Workbook.Worksheets.Copy(TemplateName, SheetName);
    }

    private void BuildReportBody(ExcelModel model)
    {
        reportSheet.ApplyModel(model.FileInfo);
        reportSheet.ApplyVertical(model.ExcelModels, StartRowIndex);
    }

    private void ClearTemplate()
    {
        excelPackage.Workbook.Worksheets.Delete(TemplateName);
    }
}