using System;
using System.IO;
using System.Threading.Tasks;
using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.Constants;
using OWP.OddsCompiler.Criteria;
using OWP.OddsCompiler.Models;
using OWP.Services;

namespace OWP.OddsCompiler.Controllers;
[Route("api/odds-compiler")]
public class OddsCompilerApiController(Repository oddsCompilerRepository, ConfigurationService configurationService,
        ExcelExportService excelExportService)
    : ControllerBase
{
    [Route("query")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.OddsCompiler, Policy = Constant.Policy.Read)]
    public async Task<IActionResult> GetOddsCompiler([FromBody] QueryModel model)
    {
        var criteria = new GetOddsCompilerCriteria
        {
            Date = model.Date,
            League = model.League,
            MatchStatusIds = string.Join(",", model.MatchStatusIds),
            IsExcludeSpecialProducts = model.IsExcludeSpecialProducts
        };

        var result = await oddsCompilerRepository.GetOddsCompiler(criteria);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("save-market")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.OddsCompiler, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> SaveMarket([FromBody] MarketUpdateModel model)
    {
        var result = await oddsCompilerRepository.SaveMarket(model);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("insert-odds")]
    [ HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.OddsCompiler, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> InsertOdds([FromBody] InsertOddsModel model)
    {
        var result = await oddsCompilerRepository.InsertOdds(model);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("update-odds")]
    [ HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.OddsCompiler, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> UpdateOdds([FromBody] UpdateOddsModel model)
    {
        var result = await oddsCompilerRepository.UpdateOdds(model);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("delete-odds")]
    [ HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.OddsCompiler, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> DeleteOdds([FromBody] DeleteOddsModel model)
    {
        var result = await oddsCompilerRepository.DeleteOdds(model);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("export-excel")]
    [HttpGet]
    [NapAuth(FunctionKey = FunctionKeys.OddsCompiler, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> ExportExcel(DateTime date, string league, string matchStatuses, bool isExcludeSpecialProducts)
    {
        const string templatePath = @"App_Data/Templates/ExportOddsCompilerTemplate.xlsx";
        var absolutedTemplatePath =  $"{configurationService.CurrentDirectory}{Path.DirectorySeparatorChar}{templatePath}";
        var criteria = new GetOddsCompilerCriteria
        {
            Date = date,
            IsExcludeSpecialProducts = isExcludeSpecialProducts,
            League = league,
            MatchStatusIds = matchStatuses
        };
        var excelModel = await excelExportService.GetExcelModel(criteria);
        using var excelPackage = excelExportService.CreateExcelPackage(absolutedTemplatePath, excelModel);
        var bytes = await excelPackage.GetAsByteArrayAsync();
        return File(bytes, "application/octet-stream", $"OddsCompiler_{ date:yyyyMMdd}.xlsx");
    }
}

