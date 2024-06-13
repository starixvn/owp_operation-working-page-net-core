using System;
using System.Threading.Tasks;
using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;
using Microsoft.AspNetCore.Mvc;
using OWP.Constants;
using OWP.OddsCompiler.Criteria;
using OWP.OddsCompiler.Models;
using OWP.Services;

namespace OWP.OddsCompiler.Controllers;
[Route("api/odds-compiler")]
public class OddsCompilerApiController : ControllerBase
{
    private readonly Repository oddsCompilerRepository;
    private readonly ExcelExportService excelExportService;
    private readonly ConfigurationService configurationService;

    public OddsCompilerApiController(Repository oddsCompilerRepository, ConfigurationService configurationService, ExcelExportService excelExportService)
    {
        this.oddsCompilerRepository = oddsCompilerRepository;
        this.configurationService = configurationService;
        this.excelExportService = excelExportService;
    }

    [Route("query")]
    [HttpGet, HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.OddsCompiler, Policy = Constant.Policy.Read)]
    public async Task<IActionResult> GetOddsCompiler(QueryModel model)
    {
        var criteria = new GetOddsCompilerCriteria
        {
            Date = model.Date,
            League = model.League,
            MatchStatusIds = string.Join(",", model.MatchStatusIds),
            IsExcludeSpecialProducts = model.IsExcludeSpecialProducts
        };

        var result = await oddsCompilerRepository.GetOddsCompiler(criteria);
        return Ok(result);
    }

    [Route("save-market")]
    [HttpGet, HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.OddsCompiler, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> SaveMarket(MarketUpdateModel model)
    {
        var result = await oddsCompilerRepository.SaveMarket(model);
        return Ok(result);
    }

    [Route("insert-odds")]
    [HttpGet, HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.OddsCompiler, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> InsertOdds(InsertOddsModel model)
    {
        var result = await oddsCompilerRepository.InsertOdds(model);
        return Ok(result);
    }

    [Route("update-odds")]
    [HttpGet, HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.OddsCompiler, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> UpdateOdds(UpdateOddsModel model)
    {
        var result = await oddsCompilerRepository.UpdateOdds(model);
        return Ok(result);
    }

    [Route("delete-odds")]
    [HttpGet, HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.OddsCompiler, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> DeleteOdds(DeleteOddsModel model)
    {
        var result = await oddsCompilerRepository.DeleteOdds(model);
        return Ok(result);
    }

    //[Route("export-excel")]
    //[HttpGet]
    //[NapAuth(FunctionKey = FunctionKeys.OddsCompiler, Policy = Constant.Policy.Host)]
    //public async Task<IActionResult> ExportExcel(DateTime date, string league, string matchStatuses, bool isExcludeSpecialProducts)
    //{
    //    const string templatePath = @"App_Data\Templates\ExportOddsCompilerTemplate.xlsx";
    //    var absolutedTemplatePath = $"{configurationService.CurrentDirectory}\\{templatePath}";
    //    var criteria = new GetOddsCompilerCriteria
    //    {
    //        Date = date,
    //        IsExcludeSpecialProducts = isExcludeSpecialProducts,
    //        League = league,
    //        MatchStatusIds = matchStatuses
    //    };
    //    var excelModel = await excelExportService.GetExcelModel(criteria);
    //    var excelPackage = excelExportService.CreateExcelPackage(absolutedTemplatePath, excelModel);
    //    return new EpplusExcelResult(excelPackage) { FileName = $"OddsCompiler_{date.ToString("yyyyMMdd")}.xlsx" };
    //}
}

